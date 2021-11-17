using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Activities
{
    /// <summary>
    /// 
    /// </summary>
   public static class ActivityDataExtensions
    {
        /// <summary>
        /// 是否在当天可操作时间区间内（活动进行中，且在当天的HourStart到HourEnd）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool CanOperate(this ActivityData  activity)
        {
            var hour = DateTime.Now.Hour;
            return activity.OnGoing() && activity.HourStart <= hour && activity.HourEnd >= hour;
        }
        /// <summary>
        /// 是否在领奖时间区间内（活动结束且在计算榜单分钟数之后到图标结束）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool CanRecieveReward(this ActivityData activity) => activity.OnShowing() && DateTime.Now >= activity.DateEnd.AddMinutes(activity.CalcuRewardMinutes);
        /// <summary>
        /// 是否已过活动图标结束时间（当前时间大于PicEnd）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool IsPicEnded(this ActivityData activity) => activity.PicEnd < DateTime.Now;
        /// <summary>
        /// 是否已过活动结束时间（当前时间大于DateEnd）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool IsEnded(this ActivityData activity) => activity.DateEnd < DateTime.Now;
        /// <summary>
        /// 活动是否进行中（DateStart到DateEnd区间）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool OnGoing(this ActivityData activity) => activity.DateEnd > DateTime.Now && activity.DateStart <= DateTime.Now;
        /// <summary>
        /// 活动是否在图标显示区间（PicStart到PicEnd）
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static bool OnShowing(this ActivityData activity) => activity.PicEnd > DateTime.Now && activity.PicStart <= DateTime.Now;

    }
}
