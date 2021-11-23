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
        public AvatarCurrency Currency { get; set; } = new  AvatarCurrency();
    }

    /// <summary>
    /// 
    /// </summary>
    public class AvatarCurrency
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
        /// 角色当前阅历
        /// </summary>
        public long Experience { get; set; }

        /// <summary>
        /// 角色通宝
        /// </summary>
        public BigInteger Money { get; set; }

        /// <summary>
        /// 当前声望
        /// </summary>
        public long Prestige { get; set; }

        /// <summary>
        /// 当前金珠
        /// </summary>
        public long Gold { get; set; }

        /// <summary>
        /// 累计获得金珠
        /// </summary>
        public long TotalGold { get; set; }

        /// <summary>
        /// 客栈-当前交子
        /// </summary>
        public long JiaoZi { get; set; }

        /// <summary>
        /// 乔迁-当前陈酿
        /// </summary>
        public long ChenNiang { get; set; }

        /// <summary>
        /// 商战-当前商战币
        /// </summary>
        public long ShangZhanBi { get; set; }
        /// <summary>
        /// 商会-当前商会贡献
        /// </summary>
        public long ShangHuiGongXian { get; set; }

        /// <summary>
        /// 酒楼-宴会币
        /// </summary>
        public long YanHuiBi { get; set; }

        /// <summary>
        /// 狩猎-狩猎积分
        /// </summary>
        public long ShouLieJiFen { get; set; }

        /// <summary>
        /// 驯灵叶
        /// </summary>
        public long XunLingYe { get; set; }
    }
}
