using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class HeroSetting : GameSettingBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名号
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 品质
        /// </summary>
        public Quality Quality { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public Profession Profession { get; set; }

        /// <summary>
        /// 介绍
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 基础资质
        /// </summary>
        public int Ability { get; set; }

        /// <summary>
        /// 美女缘分 [1#2#3]
        /// </summary>
        public List<int> Fates { get; set; }


        /// <summary>
        /// 初始技能 [1#2#3]
        /// </summary>
        public List<int> Skills { get; set; }

        /// <summary>
        /// 初始天赋配置 [1#2#3]
        /// </summary>
        public int[] Talents { get; set; }

        /// <summary>
        /// 初始默认皮肤
        /// </summary>
        public int DefaultSkin { get; set; }

        /// <summary>
        /// 体质转化系数（万分比）
        /// </summary>
        public int HpModulus { get; set; }
        /// <summary>
        /// 谋略转化系数（万分比）
        /// </summary>
        public int AtkModulus { get; set; }
        /// <summary>
        /// 经营转化系数（万分比）
        /// </summary>
        public int BiasModulus { get; set; }
        /// <summary>
        /// 名气转化系数（万分比）
        /// </summary>
        public int FameModulus { get; set; }

        /// <summary>
        /// 升星消耗
        /// </summary>
        public int StarItem { get; set; }

        /// <summary>
        /// 重复获得英雄转化碎片
        /// </summary>
        public int Chip { get; set; }

    }
}
