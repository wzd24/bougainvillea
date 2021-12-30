using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.BeautySystem
{
    /// <summary>
    /// 美女模块错误码
    /// </summary>
    public class BeautyErrorCode
    {
        /// <summary>
        /// 美女传唤失败，或精力不足，或美女信息错误
        /// </summary>
        public static int ChuanHuanFail = 203001;
        /// <summary>
        /// 道具赠送失败，或道具不足
        /// </summary>
        public static int GivingFail = 203002;
        /// <summary>
        /// 缘分技能升级失败，或缘分技能不存在，或缘分值不足
        /// </summary>
        public static int UpgradeSkillFail4Fate = 203003;
        /// <summary>
        /// 美名升级失败，或条件不足
        /// </summary>
        public static int UpgradeTitleFail4CanNot = 203004;
        /// <summary>
        /// 未解锁一键十连赠送
        /// </summary>
        public static int NotUnlockOneKeyGiftGive = 203005;
    }
}
