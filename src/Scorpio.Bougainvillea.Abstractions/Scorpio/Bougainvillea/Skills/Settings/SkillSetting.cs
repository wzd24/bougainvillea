using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Scorpio.Bougainvillea.Skills.Settings
{
    /// <summary>
    /// 技能配置数据
    /// </summary>
    public class SkillSetting:GameSettingBase
    {

        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 技能描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 技能拥有者。如：门客：1，美女：2
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// 拥有者Id
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 技能最大等级
        /// </summary>
        public int MaxLv { get; set; }

        /// <summary>
        /// 解锁条件
        /// </summary>
        public string UnLockCondition { get; set; }

        /// <summary>
        /// 技能升级消耗
        /// </summary>
        public string Depletion { get; set; }

        /// <summary>
        /// 技能效果
        /// </summary>
        public string Effect { get; set; }
    }
}
