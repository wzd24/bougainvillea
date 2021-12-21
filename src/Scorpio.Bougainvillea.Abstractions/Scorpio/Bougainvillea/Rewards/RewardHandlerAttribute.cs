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
   [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public sealed class RewardHandlerAttribute : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reward"></param>
        public RewardHandlerAttribute(params int[] reward)
        {
            Reward = reward;
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Reward { get; }
    }
}
