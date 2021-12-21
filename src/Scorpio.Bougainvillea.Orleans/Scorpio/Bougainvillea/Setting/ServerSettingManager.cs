using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using Orleans;

using Scorpio.Bougainvillea.AdoNet;
using Scorpio.Bougainvillea.Essential;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    internal class ServerSettingManager : Grain, IServerSettingManager
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly Dictionary<string, GameSettingValue> _settings = new Dictionary<string, GameSettingValue>();
        private readonly MethodInfo _getMethod = typeof(ServerSettingManager).GetMethod(nameof(GetValueAsync), BindingFlags.NonPublic | BindingFlags.Instance);
        private string _serverSettingName;
        private int _serverId;
        public ServerSettingManager(IGameSettingDefinitionManager definitionManager, IDbConnectionFactory dbConnectionFactory)
        {
            _definitionManager = definitionManager;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public override Task OnActivateAsync()
        {
            _serverId = (int)this.GetPrimaryKeyLong(out _serverSettingName);
            return base.OnActivateAsync();
        }

        public ValueTask<IReadOnlyCollection<T>> GetAsync<T>() where T : GameSettingBase
        {
            var def = _definitionManager.Get<T>();
            if (def == null || def.Scope == GameSettingScope.Default)
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
            return (await GetAsync<T>()).SingleOrDefault(s=>s.Id == id);
        }

        public async ValueTask InitializeAsync()
        {
            var definition = _definitionManager.Get(_serverSettingName);
            var method = _getMethod.MakeGenericMethod(definition.ValueType);
            var result = method.Invoke(this, new object[] { definition }) as Task<GameSettingValue>;
            var value = await result;
            _settings.AddOrUpdate(definition.Name, _ => value);
        }

        private async Task<GameSettingValue> GetValueAsync<T>(GameSettingDefinition settingDefinition)
           where T : GameSettingBase
        {
            
            if (settingDefinition.Scope == GameSettingScope.Global)
            {
                var value = await GrainFactory.GetGrain<IGlobalSettingManager>(_serverSettingName).GetAsync<T>();
                return new GameSettingValue<T>(value.ToHashSet()) { Definition = settingDefinition };
            }
            using (var conn = await _dbConnectionFactory.GetDbConnectionAsync((int)this.GetPrimaryKeyLong(), "Game_Config"))
            {
                var result = await conn.GetAllAsync<T>(new { ServerId = _serverId }, tableName: settingDefinition.Name);
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
