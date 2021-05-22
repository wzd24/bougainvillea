using System.Collections;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Rewards
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RewardHandleContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ICollection<int> Rewards { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList AdditionDatas { get; } = new List<object>();
    }
}