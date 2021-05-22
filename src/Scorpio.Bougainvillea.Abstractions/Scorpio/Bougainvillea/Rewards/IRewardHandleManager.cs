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
        /// <param name="avatarId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        Task<object> Handle(int[] reward,int avatarId,int serverId);
    }
}
