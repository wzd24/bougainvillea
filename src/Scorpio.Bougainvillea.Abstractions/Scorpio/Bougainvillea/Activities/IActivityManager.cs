using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Activities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivityManager
    {
        /// <summary>
        /// 获取本服所有图标展示中的活动
        /// </summary>
        /// <returns></returns>
        Task<List<ActivityData>> GetActivities();
        /// <summary>
        /// 获取指定ID的活动
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns></returns>
        Task<ActivityData> GetActivity(int activityId);
        /// <summary>
        /// 获取指定大类的首个正在图标展示或未来将会展示的第一个活动
        /// </summary>
        /// <param name="activityCode">活动大类</param>
        /// <returns></returns>
        Task<ActivityData> GetCurrentActivity(int activityCode);
        /// <summary>
        /// 获取指定大类和小类的首个正在图标展示或未来将会展示的第一个活动
        /// </summary>
        /// <param name="activityCode">活动大类</param>
        /// <param name="smallType">活动小类</param>
        /// <returns></returns>
        Task<ActivityData> GetCurrentActivity(int activityCode,int smallType);
        /// <summary>
        /// 更新活动的重置时间
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns></returns>
        Task<bool> UpdateResetTime(int activityId);
    }
}
