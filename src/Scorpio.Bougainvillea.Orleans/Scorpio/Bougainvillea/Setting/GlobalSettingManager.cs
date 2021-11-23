using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using Orleans;

using Scorpio.Bougainvillea.AdoNet;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    internal class GlobalSettingManager : Grain, IGlobalSettingManager
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly Dictionary<string, GameSettingValue> _settings = new Dictionary<string, GameSettingValue>();
        private readonly MethodInfo _getMethod = typeof(GlobalSettingManager).GetMethod(nameof(GetValueAsync), BindingFlags.NonPublic| BindingFlags.Instance);
        public GlobalSettingManager(IGameSettingDefinitionManager definitionManager, IDbConnectionFactory dbConnectionFactory)
        {
            _definitionManager = definitionManager;
            _dbConnectionFactory = dbConnectionFactory;
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

        public async ValueTask InitializeAsync()
        {
            var defs = _definitionManager.GetAll().Where(d => d.Scope == GameSettingScope.Global);
            await defs.ForEachAsync(async d =>
             {
                 var method = _getMethod.MakeGenericMethod(d.ValueType);
                 var result = method.Invoke(this, new object[] { d }) as Task<GameSettingValue>;
                 var value = await result;
                 _settings.AddOrUpdate(d.Name, _ => value);
             });
        }

        private async Task<GameSettingValue> GetValueAsync<T>(GameSettingDefinition settingDefinition)
           where T : GameSettingBase
        {
            using (var conn = await _dbConnectionFactory.GetDbConnectionAsync(0, "Conn_Config"))
            {
                var result = await conn.GetAllAsync<T>(tableName: settingDefinition.Name);
                if (result.IsNullOrEmpty())
                {
                    result = (settingDefinition.Default as IEnumerable<T>)??new List<T>();
                }
                return new GameSettingValue<T>(result.ToHashSet()) { Definition = settingDefinition };
            }
        }

    }
}
