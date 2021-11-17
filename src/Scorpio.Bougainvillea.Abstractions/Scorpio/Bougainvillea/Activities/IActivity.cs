using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Activities
{

    /// <summary>
    /// 活动基础接口
    /// </summary>
    public interface IActivity
    {

        /// <summary>
        /// 获取活动代码
        /// </summary>
        /// <returns></returns>
        Task<int> GetActivityCode();

        /// <summary>
        /// 该活动内的小类型（用于类似冲榜之类的活动）
        /// </summary>
        /// <returns></returns>
        Task<int> GetSmallType();

        /// <summary>
        /// 获取活动当前所处阶段
        /// </summary>
        /// <returns></returns>
        Task<ActivityStage> GetActivityStage();

        /// <summary>
        /// 活动是否显示图标
        /// </summary>
        /// <returns></returns>
        Task<bool> OnShowing();

        /// <summary>
        /// 是否在活动进行期间
        /// </summary>
        /// <returns></returns>
        Task<bool> OnGoing();

        /// <summary>
        /// 当前是否可以操作活动。
        /// </summary>
        /// <returns></returns>
        Task<bool> CanOperate();

        /// <summary>
        /// 当前是否可以获取奖励
        /// </summary>
        /// <returns></returns>
        Task<bool> CanRecieveReward();

        /// <summary>
        /// 活动是否已经结束
        /// </summary>
        /// <returns></returns>
        Task<bool> IsEnded();

        /// <summary>
        /// 获取当前玩家的数值，数据结构由具体业务确定
        /// </summary>
        /// <returns></returns>
        Task<object> GetCurrentAvatarValue();

        /// <summary>
        /// 重置活动。
        /// </summary>
        /// <returns></returns>
        Task Reset();

        /// <summary>
        /// 当前玩家上分
        /// </summary>
        /// <param name="increment"></param>
        /// <returns></returns>
        Task ScoreUp(long increment);
    }
}
