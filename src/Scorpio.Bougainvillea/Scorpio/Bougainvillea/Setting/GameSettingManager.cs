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
    internal class GameSettingManager : IGameSettingManager, ISingletonDependency
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IGameSettingProviderManager _providerManager;

        public GameSettingManager(IGameSettingDefinitionManager definitionManager, IGameSettingProviderManager providerManager)
        {
            _definitionManager = definitionManager;
            _providerManager = providerManager;
        }

        public async ValueTask<IReadOnlyCollection<T>> GetAsync<T>() where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var value = await GetGameSettingValueAsync<T>(setting);
            return value?.Value;
        }

        public async ValueTask<T> GetAsync<T>(int id) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var value = (await GetGameSettingValueAsync(setting, id));
            return value;
        }

        public async ValueTask<int> GetMaxIdAsync<T>() where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var value = await GetGameSettingMaxIdAsync<T>(setting);
            return value;
        }

        protected async ValueTask<T> GetGameSettingValueAsync<T>(GameSettingDefinition<T> setting, int id) where T : GameSettingBase
        {
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
            var value = await GetValueFromProvidersAsync<T>(providers, setting, id);
            return value?.Value?.SingleOrDefault();
        }

        protected async ValueTask<GameSettingValue<T>> GetGameSettingValueAsync<T>(GameSettingDefinition setting) where T : GameSettingBase
        {
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
            var value = await GetValueFromProvidersAsync<T>(providers, setting);
            return value;
        }

        protected async ValueTask<int> GetGameSettingMaxIdAsync<T>(GameSettingDefinition setting) where T : GameSettingBase
        {
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
            var value = await GetMaxIdFromProvidersAsync<T>(providers, setting);
            return value;
        }

        protected virtual async ValueTask<GameSettingValue<T>> GetValueFromProvidersAsync<T>(
         IEnumerable<IGameSettingProvider> providers,
         GameSettingDefinition setting) where T : GameSettingBase
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetAsync<T>(setting);
                if (value != null)
                {
                    return value;
                }
            }
            return default;
        }

        protected virtual async ValueTask<int> GetMaxIdFromProvidersAsync<T>(
            IEnumerable<IGameSettingProvider> providers,
            GameSettingDefinition setting) where T : GameSettingBase
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetMaxIdAsync<T>(setting);
                if (value != 0)
                {
                    return value;
                }
            }
            return default;
        }


        protected virtual async ValueTask<GameSettingValue<T>> GetValueFromProvidersAsync<T>(
            IEnumerable<IGameSettingProvider> providers,
            GameSettingDefinition setting, int id) where T : GameSettingBase
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetAsync<T>(setting, id);
                if (value != null)
                {
                    return value;
                }
            }
            return default;
        }

    }
}
