using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential
{
    partial class AvatarState
    {
        /// <summary>
        /// 
        /// </summary>
        public AvatarColdData ColdData { get; set; } = new AvatarColdData();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AvatarColdData
    {
        /// <summary>
        /// 玩家角色编号
        /// </summary>
        public long AvatarId { get; set; }
        /// <summary>
        /// 所属服务器Id
        /// </summary>
        public int ServerId { get; set; }
        /// <summary>
        /// 等待处理的奖励
        /// </summary>
        public Dictionary<int, SystemRewardInfo> WaitRewardDic { get; set; }= new Dictionary<int, SystemRewardInfo>();
        /// <summary>
        /// 最大奖励id
        /// </summary>
        public int MaxWaitRewardId { get; set; }

        #region 商会

        /// <summary>
        /// 商会ID
        /// </summary>
        public long GuildId { set; get; }
        /// <summary>
        /// 商会的公告时间
        /// </summary>
        public DateTime AnnounceTime { set; get; }
        /// <summary>
        /// 最后一次离开商会的时间
        /// </summary>
        public DateTime LeaveGuildTime { set; get; }
        /// <summary>
        /// 申请入会的商会ID列表
        /// </summary>
        public List<long> RequestGuildIds { set; get; }= new List<long>();

        #endregion

        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 设备系统
        /// </summary>
        public string DeviceOS { get; set; }
        /// <summary>
        /// 设备版本
        /// </summary>
        public string DeviceVer { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// 大平台信息
        /// </summary>
        public string PlatformID { get; set; }
        /// <summary>
        /// 子平台信息
        /// </summary>
        public string SmallPlatformID { get; set; }
        /// <summary>
        /// 是否运维帐号
        /// </summary>
        public short IsOperation { get; set; }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public string CVer { get; set; }
        /// <summary>
        /// 新手引导ID
        /// </summary>
        public int GuideId { get; set; }
        /// <summary>
        /// 注册ip地址
        /// </summary>
        public string RegisterIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MyProperty { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        public string LoginIp { get; set; }
        /// <summary>
        /// 封禁结束时间
        /// </summary>
        public DateTime ForbidEndTime { get; set; }
        /// <summary>
        /// 是否删除角色 0 正常 1 已删除
        /// </summary>
        public int IsRemove { get; set; }
        /// <summary>
        /// 创角七日奖励
        /// </summary>
        public List<int> ReceiveCreateReward { get; set; }= new List<int>();
        /// <summary>
        /// 创角七日登录天数
        /// </summary>
        public int Create7DayLoginDay { get; set; }
        /// <summary>
        /// 剧情引导ID
        /// </summary>
        public int StoryId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 注册登录id
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 系统语言
        /// </summary>
        public string OSLanguage { get; set; }
        /// <summary>
        /// 同设备/同账号设备编号
        /// </summary>
        public string MultiAccount { get; set; }
        /// <summary>
        /// SDK账号id
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// 商会引导ID
        /// </summary>
        public int HadGuideGuild { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SystemRewardInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public SystemRewardInfo()
        {
            RewardInfo = new List<List<int>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<List<int>> RewardInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
    }
}
