using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameSettingProvider : IGameSettingProvider, ISingletonDependency
    {
        private readonly IGameSettingStore _settingStore;

        /// <summary>
        /// 
        /// </summary>
        public abstract GameSettingScope Scope { get; }

        private readonly ConcurrentDictionary<string, GameSettingValue> _cachedValues;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingStore"></param>
        protected GameSettingProvider(IGameSettingStore settingStore)
        {
            _settingStore = settingStore;
            _cachedValues = new ConcurrentDictionary<string, GameSettingValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        public async Task<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition<T> settingDefinition)
            where T : class
        {
            if (!(_cachedValues.GetOrDefault(settingDefinition.Name) is GameSettingValue<T> value))
            {
                var context = CreateContext(settingDefinition);
                value = await _settingStore.GetAsync<T>(context);
                if (value == null)
                {
                    return null;
                }
                _cachedValues.TryAdd(settingDefinition.Name, value);
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, string key, T value) where T : class
        {
            await SetCoreAsync(settingDefinition, key, value);
            if (_cachedValues.ContainsKey(settingDefinition.Name))
            {
                _cachedValues.Remove(settingDefinition.Name, out _);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, IReadOnlyDictionary<string, T> values) where T : class
        {
            await values.ForEachAsync(async v => await SetCoreAsync(settingDefinition, v.Key, v.Value));
            if (_cachedValues.ContainsKey(settingDefinition.Name))
            {
                _cachedValues.Remove(settingDefinition.Name, out _);
            }
        }

        private async Task SetCoreAsync<T>(GameSettingDefinition<T> settingDefinition, string key, T value) where T : class
        {
            var context = CreateContext(settingDefinition);
            context.Key = key;
            await _settingStore.SetAsync(context, value);
        }

        private GameSettingStoreContext CreateContext<T>(GameSettingDefinition<T> settingDefinition) where T : class
        {
            var context = new GameSettingStoreContext(settingDefinition);
            ConfigContext(context);
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ConfigContext(IGameSettingStoreContext context)
        {

        }
    }
}
