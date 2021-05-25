using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingManager : IGameSettingManager, ISingletonDependency
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IGameSettingProviderManager _providerManager;

        public GameSettingManager(IGameSettingDefinitionManager definitionManager, IGameSettingProviderManager providerManager)
        {
            _definitionManager = definitionManager;
            _providerManager = providerManager;
        }
        public async Task<IReadOnlyDictionary<int, T>> GetAsync<T>() where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
            var value = await GetValueFromProvidersAsync(providers, setting);
            return value;
        }

        public async Task SetAsync<T>(int key, T value) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, key, value));

        }

        public async Task SetAsync<T>(IReadOnlyDictionary<int, T> values) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, values));
        }

        protected virtual async Task<IReadOnlyDictionary<int, T>> GetValueFromProvidersAsync<T>(
         IEnumerable<IGameSettingProvider> providers,
         GameSettingDefinition<T> setting) where T : GameSettingBase
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetAsync(setting);
                if (value != null)
                {
                    return value.Value;
                }
            }
            return default;
        }
    }
}
