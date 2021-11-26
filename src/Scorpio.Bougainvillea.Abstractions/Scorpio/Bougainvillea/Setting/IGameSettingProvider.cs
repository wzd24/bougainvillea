using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public GameSettingScope Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        Task<GameSettingValue> GetAsync(GameSettingDefinition settingDefinition);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, T value) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, IReadOnlyCollection<T> values) where T : GameSettingBase;
    }
}
