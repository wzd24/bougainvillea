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
    public interface IServerSettingManager : IGrainWithIntegerKey
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

    }
}
