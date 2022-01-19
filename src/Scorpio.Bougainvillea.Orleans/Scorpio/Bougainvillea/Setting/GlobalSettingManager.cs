using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using Newtonsoft.Json.Linq;

using Orleans;

using Scorpio.Bougainvillea.AdoNet;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting.StreamDatas;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    internal class GlobalSettingManager : GrainBase, IGlobalSettingManager
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly Dictionary<string, GameSettingValue> _settings = new Dictionary<string, GameSettingValue>();
        private readonly MethodInfo _getMethod = typeof(GlobalSettingManager).GetMethod(nameof(GetValueAsync), BindingFlags.NonPublic | BindingFlags.Instance);
        private string _settingName;

        public GlobalSettingManager(IServiceProvider serviceProvider, IGameSettingDefinitionManager definitionManager, IDbConnectionFactory dbConnectionFactory)
            : base(serviceProvider)
        {
            _definitionManager = definitionManager;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public override Task OnActivateAsync()
        {
            _settingName = this.GetPrimaryKeyString();
            return base.OnActivateAsync();
        }



        public ValueTask<IReadOnlyCollection<T>> GetAsync<T>() where T : GameSettingBase
        {
            var def = _definitionManager.Get<T>();
            if (def == null || def.Scope != GameSettingScope.Global)
            {
                return ValueTask.FromResult<IReadOnlyCollection<T>>(default);
            }
            var value = _settings.GetOrDefault(def.Name);
            if (value != null && value is GameSettingValue<T> tv)
            {
                return ValueTask.FromResult(tv.Value);
            }
            return ValueTask.FromResult<IReadOnlyCollection<T>>(default);
        }

        public async ValueTask<T> GetAsync<T>(int id) where T : GameSettingBase
        {
            return (await GetAsync<T>()).SingleOrDefault(s => s.Id == id);
        }

        public async ValueTask InitializeAsync()
        {
            var definition = _definitionManager.Get(_settingName);
            if (definition == null)
            {
                return;
            }
            if (definition.Scope != GameSettingScope.Global)
            {
                return;
            }
            var method = _getMethod.MakeGenericMethod(definition.ValueType);
            var result = method.Invoke(this, new object[] { definition }) as Task<GameSettingValue>;
            var value = await result;
            _settings.AddOrUpdate(definition.Name, _ => value);
            await this.GetStreamAsync<SettingInitializationData>(Guid.Empty, $"Setting.Initailization.{_settingName}").OnNextAsync(new SettingInitializationData { SettingName = _settingName });
        }

        private async Task<GameSettingValue> GetValueAsync<T>(GameSettingDefinition settingDefinition)
           where T : GameSettingBase
        {
            using (var conn = await _dbConnectionFactory.GetDbConnectionAsync(0, "Conn_Config"))
            {
                var result = await conn.GetAllAsync<T>(tableName: settingDefinition.Name);
                if (result.IsNullOrEmpty())
                {
                    result = (settingDefinition.Default as IEnumerable<T>) ?? new List<T>();
                }
                return new GameSettingValue<T>(result.ToHashSet()) { Definition = settingDefinition };
            }
        }

        public async ValueTask<int> GetMaxIdAsync<T>() where T : GameSettingBase
        {
            var values = await GetAsync<T>();
            if (values.IsNullOrEmpty())
            {
                return 0;
            }
            return values.Max(x => x.Id);
        }
    }
}
