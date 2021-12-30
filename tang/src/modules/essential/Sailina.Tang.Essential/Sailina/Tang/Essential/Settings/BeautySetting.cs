using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class BeautySetting:GameSettingBase
    {
        /// <summary>
        /// 美女名称
        /// </summary>
        public string BeautyName { get; set; }

        /// <summary>
        /// 美女性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public Profession Profession { get; set; }

        /// <summary>
        /// 品质
        /// </summary>
        public Quality Quality { get; set; }

        /// <summary>
        /// 子嗣初始品质
        /// </summary>
        public KidQuality KidQuality { get; set; }

        /// <summary>
        /// 初始亲密度
        /// </summary>
        public int Intimacy { get; set; }

        /// <summary>
        /// 初始魅力值
        /// </summary>
        public int Charm { get; set; }

        /// <summary>
        /// 初始派遣技能 [1#2#3]
        /// </summary>
        public List<int> ShopSkills { get; set; }

        /// <summary>
        /// 初始缘分技能
        /// </summary>
        public List<int> FateSkills { get; set; }

        /// <summary>
        /// 初始天赋配置 [1#2#3]
        /// </summary>
        public int[] Talents { get; set; }

        /// <summary>
        /// 重复获得美女转化碎片
        /// </summary>
        public int Chip { get; set; }

        /// <summary>
        /// 初始默认皮肤
        /// </summary>
        public int DefaultSkin { get; set; }

        /// <summary>
        /// 获得子嗣概率
        /// </summary>
        public int KidWeights { get; set; }

        /// <summary>
        /// 传唤权重
        /// </summary>
        public int CallWeights { get; set; }

        /// <summary>
        /// 好感度
        /// </summary>
        public int Favor { get; set; }

        /// <summary>
        /// 待字闺中过滤判断
        /// </summary>
        public int TravelGet { get; set; }

        /// <summary>
        /// 升星消耗
        /// </summary>
        public int StarItem { get; set; }

        /// <summary>
        /// 获取说明
        /// </summary>
        public int Condition { get; set; }

        /// <summary>
        /// 获取说明值
        /// </summary>
        public int ConditionValue { get; set; }

        /// <summary>
        /// 美女描述
        /// </summary>
        public string Des { get; set; }
    }
}
