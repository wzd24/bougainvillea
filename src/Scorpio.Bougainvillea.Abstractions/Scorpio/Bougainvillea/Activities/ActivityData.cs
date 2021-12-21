using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Activities
{
    /// <summary>
    /// 游戏活动数据
    /// </summary>
    public class ActivityData
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// 活动代码
        /// </summary>
        public int ActivityCode { get; set; }

        /// <summary>
        /// 该活动内的小类型（用于类似冲榜之类的活动）
        /// </summary>
        public int SmallType { get; set; }

        /// <summary>
        /// 活动配置ID（同一小类型下不同的配置，如同一个活动配置不同的奖励，供后台发送活动时选择）
        /// </summary>
        public int ConfigId { get; set; }

        /// <summary>
        /// 区服ID
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        ///  当天活动开始时间点
        /// </summary>
        public short HourStart { get; set; }


        /// <summary>
        /// 计算奖励倒计时（分钟数）
        /// </summary>
        public short CalcuRewardMinutes { get; set; }

        /// <summary>
        /// 活动结束后重新计算数据的时间
        /// </summary>
        public DateTime EndTime_Calcu { get; set; }

        /// <summary>
        /// 活动结束时间（带有时分秒，一定要判断）
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// 活动开始时间（带有时分秒，一定要判断）
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// 活动图标消失时间
        /// </summary>
        public DateTime PicEnd { get; set; }

        /// <summary>
        /// 活动图标出现时间
        /// </summary>
        public DateTime PicStart { get; set; }

        ///// <summary>
        ///// 活动内容说明文本
        ///// </summary>
        //public string Content { get; set; }

        ///// <summary>
        ///// 活动标题
        ///// </summary>
        //public string Title { get; set; }

        /// <summary>
        /// 活动名称对应语言表的ID
        /// </summary>
        public int Name { get; set; }

        /// <summary>
        /// 当天活动结束时间点
        /// </summary>
        public short HourEnd { get; set; }

        /// <summary>
        /// 活动重置时间（用于判断是否要进行重置）
        /// </summary>
        public DateTime ResetTime { get; set; }
        /// <summary>
        /// ID主键
        /// </summary>
        public int Id { get; set; }
    }

    /// <summary>
    /// 活动所在阶段
    /// </summary>
    public enum ActivityStage
    {
        /// <summary>
        /// 未知阶段
        /// </summary>
        Unknow,
        /// <summary>
        /// 尚未开始
        /// </summary>
        BeforeShown,
        /// <summary>
        /// 显示图标
        /// </summary>
        Showing,

        /// <summary>
        /// 进行中
        /// </summary>
        Going,
        /// <summary>
        /// 正在计算奖励
        /// </summary>
        CalcuReward,

        /// <summary>
        /// 获取奖励中
        /// </summary>
        ReceiveReward,

        /// <summary>
        /// 活动结束
        /// </summary>
        Ended
    }
}
