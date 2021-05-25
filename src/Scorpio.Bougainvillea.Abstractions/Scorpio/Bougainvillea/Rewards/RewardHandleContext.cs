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
        /// 奖励参数
        /// </summary>
        public ICollection<int> Rewards { get; set; }

        /// <summary>
        /// 附加数据。
        /// </summary>
        public IList AdditionDatas { get; } = new List<object>();

        /// <summary>
        /// 奖励次数
        /// </summary>
        public int Num { get; set; }
    }
}