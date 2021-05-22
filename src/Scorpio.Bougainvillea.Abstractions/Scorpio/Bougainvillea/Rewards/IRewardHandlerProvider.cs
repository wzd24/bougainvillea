using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Rewards
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRewardHandlerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rewards"></param>
        /// <returns></returns>
        IRewardHandler GetHandler(int[] rewards);
    }
}
