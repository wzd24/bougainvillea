using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Skills
{
    /// <summary>
    /// 技能增幅效果值
    /// </summary>
    public class SkillBuffValue
    {

        /// <summary>
        /// 技能Id
        /// </summary>
        public int SkillId { get; set; }

        /// <summary>
        /// 技能等级
        /// </summary>
        public int SkillLv { get; set; }

        /// <summary>
        /// 技能增幅类型
        /// </summary>
        public SkillBuffType BuffType { get; set; }

        /// <summary>
        /// 增幅值
        /// </summary>
        public long BuffValue { get; set; }
    }

    /// <summary>
    /// 技能增幅类型
    /// </summary>
    public enum SkillBuffType
    {
        /// <summary>
        /// 绝对增幅值。
        /// </summary>
        Absolute,

        /// <summary>
        /// 增幅万分比
        /// </summary>
        Relative
    }
}
