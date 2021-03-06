using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGlobalSettingManager:IGrainWithStringKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask InitializeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<IReadOnlyCollection<T>> GetAsync<T>() where T:GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<T> GetAsync<T>(int id) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<int> GetMaxIdAsync<T>() where T : GameSettingBase;
    }
}
