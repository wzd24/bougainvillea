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
        /// <returns></returns>
        Task<(int code,object data)> Handle(int[] reward,int num);
    }
}
