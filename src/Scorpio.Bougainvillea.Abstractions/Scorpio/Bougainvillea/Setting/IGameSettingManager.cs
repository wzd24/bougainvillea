using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IReadOnlyDictionary<int, T>> GetAsync<T>() where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(int key, T value) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        Task SetAsync<T>(IReadOnlyDictionary<int,T> values) where T : GameSettingBase;
    }
}
