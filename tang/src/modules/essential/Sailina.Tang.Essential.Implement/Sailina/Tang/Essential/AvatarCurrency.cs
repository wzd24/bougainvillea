using System.Data;
using System.Numerics;

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
        public AvatarCurrency Currency { get; set; } = new AvatarCurrency();
    }

    /// <summary>
    /// 
    /// </summary>
    internal class AvatarCurrency
    {
        /// <summary>
        /// 玩家角色编号
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual long AvatarId { get; set; }

        /// <summary>
        /// 角色当前阅历
        /// </summary>
        public virtual double Experience { get; set; }

        /// <summary>
        /// 角色通宝
        /// </summary>
        public virtual double Money { get; set; }


        /// <summary>
        /// 当前声望
        /// </summary>
        public virtual double Prestige { get; set; }

        /// <summary>
        /// 当前金珠
        /// </summary>
        public virtual long Gold { get; set; }

        /// <summary>
        /// 累计获得金珠
        /// </summary>
        public virtual double TotalGold { get; set; }

        /// <summary>
        /// 客栈-当前交子
        /// </summary>
        public virtual double JiaoZi { get; set; }

        /// <summary>
        /// 乔迁-当前陈酿
        /// </summary>
        public virtual double ChenNiang { get; set; }

        /// <summary>
        /// 商战-当前商战币
        /// </summary>
        public virtual double ShangZhanBi { get; set; }
        /// <summary>
        /// 商会-当前商会贡献
        /// </summary>
        public virtual double ShangHuiGongXian { get; set; }

        /// <summary>
        /// 酒楼-宴会币
        /// </summary>
        public virtual double YanHuiBi { get; set; }

        /// <summary>
        /// 狩猎-狩猎积分
        /// </summary>
        public virtual double ShouLieJiFen { get; set; }

        /// <summary>
        /// 驯灵叶
        /// </summary>
        public virtual double XunLingYe { get; set; }

        internal static async ValueTask<AvatarCurrency> InitializeAsync(GridReader r) => (await r.ReadSingleOrDefaultAsync<AvatarCurrency>())?.GenerateProxy();

        internal async ValueTask WriteAsync(IDbConnection conn) => await this.Action(!(this is IModifiable { Modified: false }), async a => await conn.InsertOrUpdateAsync<AvatarCurrency>(this));
    }
}
