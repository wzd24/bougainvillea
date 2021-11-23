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
        private readonly MethodInfo _getMethod = typeof(GlobalSettingManager).GetMethod(nameof(GetValueAsync));
        public ServerSettingManager(IGameSettingDefinitionManager definitionManager, IDbConnectionFactory dbConnectionFactory)
        {
            _definitionManager = definitionManager;
            _dbConnectionFactory = dbConnectionFactory;
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

        public async ValueTask InitializeAsync()
        {
            var defs = _definitionManager.GetAll().Where(d => d.Scope != GameSettingScope.Default);
            await defs.ForEachAsync(async d =>
             {
                 var method = _getMethod.MakeGenericMethod(d.ValueType);
                 var result = method.Invoke(this, new object[] { d }) as Task<GameSettingValue>;
                 var value = await result;
                 _settings.AddOrUpdate(d.Name, _ => value);
             });
        }

        private async Task<GameSettingValue<T>> GetValueAsync<T>(GameSettingDefinition settingDefinition)
           where T : GameSettingBase
        {
            if (settingDefinition.Scope == GameSettingScope.Global)
            {
                var value = await GrainFactory.GetGrain<IGlobalSettingManager>(Guid.Empty).GetAsync<T>();
                return new GameSettingValue<T>(value.ToHashSet()) { Definition = settingDefinition };
            }
            using (var conn = await _dbConnectionFactory.GetDbConnectionAsync((int)this.GetPrimaryKeyLong(), "Game_Config"))
            {
                var result = await conn.GetAllAsync<T>(new { ServerId = (int)this.GetPrimaryKeyLong() }, tableName: settingDefinition.Name);
                return new GameSettingValue<T>(result.ToHashSet()) { Definition = settingDefinition };
            }
        }

    }
}
