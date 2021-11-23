using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;
using Scorpio.Initialization;

using static Dapper.SqlMapper;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingManager : IGameSettingManager,  ISingletonDependency
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IGameSettingProviderManager _providerManager;

        public GameSettingManager(IGameSettingDefinitionManager definitionManager, IGameSettingProviderManager providerManager)
        {
            _definitionManager = definitionManager;
            _providerManager = providerManager;
        }

        public async Task<IReadOnlyCollection<T>> GetAsync<T>() where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var value = (await GetGameSettingValueAsync(setting)) as GameSettingValue<T>;
            return value?.Value;
        }



        protected async Task<GameSettingValue> GetGameSettingValueAsync(GameSettingDefinition setting)
        {
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
            var value = await GetValueFromProvidersAsync(providers, setting);
            return value;
        }

        public async Task SetAsync<T>(T value) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, value));
        }

        public async Task SetAsync<T>(IReadOnlyCollection<T> values) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, values));
        }

        protected virtual async Task<GameSettingValue> GetValueFromProvidersAsync(
         IEnumerable<IGameSettingProvider> providers,
         GameSettingDefinition setting)
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetAsync(setting);
                if (value != null)
                {
                    return value;
                }
            }
            return default;
        }

    }
}
