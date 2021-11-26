using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential
{
    partial class AvatarState
    {
        /// <summary>
        /// 
        /// </summary>
        public AvatarHotData HotData { get; set; }=new AvatarHotData();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AvatarHotData
    {
        /// <summary>
        /// 玩家角色编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 玩家登录状态
        /// </summary>
        public bool LoginStatus { get; set; }
        /// <summary>
        /// 上一次登录时间
        /// </summary>
        public DateTimeOffset LastLoginTime { get; set; }
        /// <summary>
        /// 最后离线时间
        /// </summary>
        public DateTimeOffset LastOfflineTime { get; set; }
        /// <summary>
        /// 每日奖励领取状态
        /// </summary>
        public bool IsReceive { get; set; }
        /// <summary>
        /// 每日数据重置时间
        /// </summary>
        public DateTimeOffset ResetTime { get; set; }
        /// <summary>
        /// 当前赚速
        /// </summary>
        public double EarnSpeed { get; set; }
        /// <summary>
        /// 历史最高赚速
        /// </summary>
        public double TotalEarnSpeed { get; set; }
        /// <summary>
        /// 财神送财今日次数
        /// </summary>
        public int SendMoneyTimes { get; set; }
        /// <summary>
        /// 今日已领取财神送财奖励
        /// </summary>
        public List<int> ReceiveGradeIds { get; set; } = new List<int>();
        /// <summary>
        /// 最后更新财神送财时间
        /// </summary>
        public DateTime FinallyTime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }
        /// <summary>
        /// 累计在线时长(分钟数)
        /// </summary>
        public long OnLineTime { get; set; }
    }
}
