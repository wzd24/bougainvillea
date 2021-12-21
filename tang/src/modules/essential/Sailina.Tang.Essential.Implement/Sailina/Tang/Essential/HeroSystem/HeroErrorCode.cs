using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.HeroSystem
{
    /// <summary>
    /// 名士模块错误码
    /// </summary>
    public class HeroErrorCode
    {
        /// <summary>
        /// 等级未达到研修要求
        /// </summary>
        public static int NotEnoughtStudyLevel = 202001;
        /// <summary>
        /// 不存在该技能
        /// </summary>
        public static int NotExistSkill = 202002;
        /// <summary>
        /// 上阵槽位不足
        /// </summary>
        public static int NotEnoughtTeamPlace = 202003;
        /// <summary>
        /// 重修失败
        /// </summary>
        public static int ResetFail = 202004;
        /// <summary>
        /// 技能未解锁
        /// </summary>
        public static int NotUnlockSkill = 202005;
        /// <summary>
        /// 未解锁名士
        /// </summary>
        public static int UnlockedHero = 202006;
        /// <summary>
        /// 上阵失败，名士已上阵
        /// </summary>
        public static int TeamPlaceFail4AlreadyHero = 202007;
        /// <summary>
        /// 下阵失败，下阵时出现错误
        /// </summary>
        public static int TeamPlaceFail4Exit = 202008;
    }
}
