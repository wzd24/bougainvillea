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
    internal class GameSettingManager : IGameSettingManager, Initialization.IInitializable, ISingletonDependency
    {
        private readonly IGameSettingDefinitionManager _definitionManager;
        private readonly IGameSettingProviderManager _providerManager;
        private readonly ConcurrentDictionary<string, GameSettingValue> _cachedValues;

        public GameSettingManager(IGameSettingDefinitionManager definitionManager, IGameSettingProviderManager providerManager)
        {
            _definitionManager = definitionManager;
            _providerManager = providerManager;
            _cachedValues = new ConcurrentDictionary<string, GameSettingValue>();
        }

        public async Task<IEnumerable> GetAsync(string name)
        {
            var setting = _definitionManager.Get(name);
            var value=await GetGameSettingValueAsync(setting);
            return value.Value;
        }

        public async Task<IReadOnlyCollection<T>> GetAsync<T>() where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var value = (await GetGameSettingValueAsync(setting)) as GameSettingValue<T>;
            return value.Value;
        }

        protected async Task<GameSettingValue> GetGameSettingValueAsync(GameSettingDefinition setting)
{
            if (!(_cachedValues.GetOrDefault(setting.Name) is GameSettingValue value))
{
                var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope || p.Scope == GameSettingScope.Default).Reverse();
                value = await GetValueFromProvidersAsync(providers, setting);
                if (value == null)
                {
                    return null;
}
                _cachedValues.TryAdd(setting.Name, value);
            }
            return value;
        }

        public void Initialize()
        {
            var definitions = _definitionManager.GetAll().Where(d => d.Scope == GameSettingScope.Global);
            definitions.ForEach(d =>
            {
                _ = GetGameSettingValueAsync(d).ConfigureAwait(false).GetAwaiter().GetResult();
            });
        }

        public Task ReloadCachesAsync()
        {
            _cachedValues.Clear();
            return Task.CompletedTask;
        }

        public async Task SetAsync<T>(T value) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, value));
            if (_cachedValues.ContainsKey(setting.Name))
            {
                _cachedValues.Remove(setting.Name, out _);
            }
        }

        public async Task SetAsync<T>(IReadOnlyCollection<T> values) where T : GameSettingBase
        {
            var setting = _definitionManager.Get<T>();
            var providers = _providerManager.Providers.Where(p => p.Scope == setting.Scope);
            await providers.ForEachAsync(f => f.SetAsync(setting, values));
            if (_cachedValues.ContainsKey(setting.Name))
            {
                _cachedValues.Remove(setting.Name, out _);
            }
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
