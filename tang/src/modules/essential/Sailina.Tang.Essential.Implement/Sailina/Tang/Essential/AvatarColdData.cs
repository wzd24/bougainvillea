using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using EasyMigrator;

using Scorpio.Bougainvillea.Data;

using static Dapper.SqlMapper;

namespace Sailina.Tang.Essential
{
    internal partial class AvatarState
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
    internal class AvatarColdData
    {
        /// <summary>
        /// 玩家角色编号
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual long AvatarId { get; set; }


        /// <summary>
        /// 等待处理的奖励
        /// </summary>
        [DbType(DbType.String),Max, Default(null)]
        public virtual Dictionary<int, SystemRewardInfo> WaitRewards { get; set; } = new Dictionary<int, SystemRewardInfo>();
        /// <summary>
        /// 最大奖励id
        /// </summary>
        public virtual int MaxWaitRewardId { get; set; }

        #region 商会

        /// <summary>
        /// 商会ID
        /// </summary>
        public virtual long GuildId { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [Length(100)]
        public virtual string GuildName { get; set; }


        /// <summary>
        /// 商会的公告时间
        /// </summary>
        [DbType(DbType.Int64)]
        public virtual DateTime AnnounceTime { set; get; }
        /// <summary>
        /// 最后一次离开商会的时间
        /// </summary>
        [DbType(DbType.Int64)]
        public virtual DateTime LeaveGuildTime { set; get; }
        /// <summary>
        /// 申请入会的商会ID列表
        /// </summary>
        [DbType(DbType.String), Max,Default(null)]
        public virtual List<long> RequestGuildIds { set; get; } = new List<long>();

        #endregion

        /// <summary>
        /// 设备类型
        /// </summary>
        [Length(100)]
        public virtual string DeviceType { get; set; }
        /// <summary>
        /// 设备系统
        /// </summary>
        [Length(100)]
        public virtual string DeviceOS { get; set; }
        /// <summary>
        /// 设备版本
        /// </summary>
        [Length(100)]
        public virtual string DeviceVer { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        [Length(100)]
        public virtual string DeviceID { get; set; }
        /// <summary>
        /// 大平台信息
        /// </summary>
        [Length(100)]
        public virtual string PlatformID { get; set; }
        /// <summary>
        /// 子平台信息
        /// </summary>
        [Length(100)]
        public virtual string SmallPlatformID { get; set; }
        /// <summary>
        /// 是否运维帐号
        /// </summary>
        public virtual short IsOperation { get; set; }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        [Length(200)]
        public virtual string CVer { get; set; }
        /// <summary>
        /// 新手引导ID
        /// </summary>
        public virtual int GuideId { get; set; }
        /// <summary>
        /// 注册ip地址
        /// </summary>
        [Length(60)]
        public virtual string RegisterIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int MyProperty { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        [Length(60)]
        public virtual string LoginIp { get; set; }
        /// <summary>
        /// 封禁结束时间
        /// </summary>
        [DbType(DbType.Int64)]
        public virtual DateTime ForbidEndTime { get; set; }
        /// <summary>
        /// 是否删除角色 0 正常 1 已删除
        /// </summary>
        public virtual int IsRemove { get; set; }
        /// <summary>
        /// 创角七日奖励
        /// </summary>
        [DbType(DbType.String), Max, Default(null)]
        public virtual List<int> ReceiveCreateReward { get; set; } = new List<int>();
        /// <summary>
        /// 创角七日登录天数
        /// </summary>
        public virtual int Create7DayLoginDay { get; set; }
        /// <summary>
        /// 剧情引导ID
        /// </summary>
        public virtual int StoryId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [Length(100)]
        public virtual string Language { get; set; }
        /// <summary>
        /// 注册登录id
        /// </summary>
        [Length(300)]
        public virtual string SessionId { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        [Length(100)]
        public virtual string AppId { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        [Length(100)]
        public virtual string AppVersion { get; set; }
        /// <summary>
        /// 系统语言
        /// </summary>
        [Length(100)]
        public virtual string OSLanguage { get; set; }
        /// <summary>
        /// 同设备/同账号设备编号
        /// </summary>
        [Length(200)]
        public virtual string MultiAccount { get; set; }
        /// <summary>
        /// SDK账号id
        /// </summary>
        [Length(200)]
        public virtual string AccountId { get; set; }
        /// <summary>
        /// 商会引导ID
        /// </summary>
        public virtual int HadGuideGuild { get; set; }
        internal static async ValueTask<AvatarColdData> InitializeAsync(GridReader r) => (await r.ReadSingleOrDefaultAsync<AvatarColdData>())?.GenerateProxy();

        internal async ValueTask WriteAsync(IDbConnection conn) => await this.Action(!(this is IModifiable { Modified: false }), async a => await conn.InsertOrUpdateAsync<AvatarColdData>(this));
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
