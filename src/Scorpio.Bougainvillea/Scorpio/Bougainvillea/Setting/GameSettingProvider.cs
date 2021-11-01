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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingStore"></param>
        protected GameSettingProvider(IGameSettingStore settingStore)
        {
            _settingStore = settingStore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        public async Task<GameSettingValue> GetAsync(GameSettingDefinition settingDefinition)
        {
            var context = CreateContext(settingDefinition);
            var value = await _settingStore.GetAsync(context);
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
        public async Task SetAsync<T>(GameSettingDefinition<T> settingDefinition,  T value) where T : GameSettingBase
        {
            await SetCoreAsync(settingDefinition, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, IReadOnlyCollection<T> values) where T : GameSettingBase
        {
            await values.ForEachAsync(async v => await SetCoreAsync(settingDefinition, v));
        }

        private async Task SetCoreAsync<T>(GameSettingDefinition<T> settingDefinition,  T value) where T : GameSettingBase
        {
            var context = CreateContext(settingDefinition);
            await _settingStore.SetAsync(context, value);
        }

        private GameSettingStoreContext CreateContext(GameSettingDefinition settingDefinition)
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
