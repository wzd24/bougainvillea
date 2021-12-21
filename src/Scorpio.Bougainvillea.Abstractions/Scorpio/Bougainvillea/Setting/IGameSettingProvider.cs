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
        ValueTask<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition settingDefinition) where T:GameSettingBase;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ValueTask<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition setting, int id) where T:GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setting"></param>
        /// <returns></returns>
        ValueTask<int> GetMaxIdAsync<T>(GameSettingDefinition setting) where T : GameSettingBase;
    }
}
