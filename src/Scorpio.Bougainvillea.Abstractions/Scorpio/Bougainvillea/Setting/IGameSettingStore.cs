using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<GameSettingValue<T>> GetAsync<T>(IGameSettingStoreContext context) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(IGameSettingStoreContext context, T value) where T : GameSettingBase;
    }
}
