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
    public interface IRewardHandleManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reward"></param>
        /// <param name="num"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<(int code,object data)> HandleAsync(int[] reward,int num,string reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rewards"></param>
        /// <param name="num"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task<(int code, object data)> HandleAsync(string rewards, int num, string reason);

    }
}
