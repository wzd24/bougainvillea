using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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
        public AvatarHotData HotData { get; set; } = new AvatarHotData();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class AvatarHotData
    {
        public AvatarHotData()
        {
        }
        /// <summary>
        /// 玩家角色编号
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual long AvatarId { get; set; }
        /// <summary>
        /// 玩家登录状态
        /// </summary>
        public virtual bool LoginStatus { get; set; }
        /// <summary>
        /// 上一次登录时间
        /// </summary>
        [DbType(System.Data.DbType.DateTime)]
        public virtual DateTime LastLoginTime { get; set; }
        /// <summary>
        /// 最后离线时间
        /// </summary>
        [DbType(System.Data.DbType.DateTime)]
        public virtual DateTime LastOfflineTime { get; set; }
        /// <summary>
        /// 每日奖励领取状态
        /// </summary>
        public virtual bool IsReceive { get; set; }
        /// <summary>
        /// 每日数据重置时间
        /// </summary>
        [DbType(System.Data.DbType.DateTime)]
        public virtual DateTime ResetTime { get; set; }

        /// <summary>
        /// 当前赚速
        /// </summary>
        public virtual double EarnSpeed { get; set; }
        /// <summary>
        /// 历史最高赚速
        /// </summary>
        public virtual double TotalEarnSpeed { get; set; }
        /// <summary>
        /// 财神送财今日次数
        /// </summary>
        public virtual int SendMoneyTimes { get; set; }
        /// <summary>
        /// 今日已领取财神送财奖励
        /// </summary>
        [DbType(DbType.String),Max,Default(null)]
        public virtual List<int> ReceiveGradeIds { get; set; } = new List<int>();
        /// <summary>
        /// 最后更新财神送财时间
        /// </summary>
        [DbType(System.Data.DbType.DateTime)]
        public virtual DateTime FinallyTime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public virtual int LoginTimes { get; set; }
        /// <summary>
        /// 累计在线时长(分钟数)
        /// </summary>
        public virtual long OnLineTime { get; set; }

        internal static async ValueTask<AvatarHotData> InitializeAsync(GridReader dataReader) => (await dataReader.ReadSingleOrDefaultAsync<AvatarHotData>())?.GenerateProxy();
        internal async ValueTask WriteAsync(IDbConnection conn) => await this.Action(!(this is IModifiable { Modified: false }), async a => await conn.InsertOrUpdateAsync<AvatarHotData>(this));
    }
}
