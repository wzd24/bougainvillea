using System.Collections;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Rewards
{
    /// <summary>
    /// 通用奖励处理上下文
    /// </summary>
    public sealed class RewardHandleContext
    {
        /// <summary>
        /// 奖励参数
        /// </summary>
        public int[] Rewards { get; set; }

        /// <summary>
        /// 附加数据。
        /// </summary>
        public IList AdditionDatas { get; } = new List<object>();

        /// <summary>
        /// 奖励次数
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 奖励原因
        /// </summary>
        public string Reason { get; set; }
    }
}