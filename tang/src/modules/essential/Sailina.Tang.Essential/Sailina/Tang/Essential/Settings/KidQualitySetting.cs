using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 子嗣品质配置
    /// </summary>
    [Serializable]
    public class KidQualitySetting:GameSettingBase
    {
        /// <summary>
        /// 子嗣品质名称
        /// </summary>
        public string NameKidQuality { get; set; }

        /// <summary>
        /// 子嗣品质赚速加成最终值（万分比）
        /// </summary>
        public int AddRateEarningBonus { get; set; }

        /// <summary>
        /// 成年道具奖励数量
        /// </summary>
        public string Rewards { get; set; }

        /// <summary>
        /// 联姻道具奖励数量
        /// </summary>
        public string MarriageReward { get; set; }

        /// <summary>
        /// 品质培养时长
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 给名士增加资质
        /// </summary>
        public List<int> AddAptitude{get; set; }

        /// <summary>
        /// 子嗣孤独终老加成
        /// </summary>
        public List<int> KidDieAloneBnous{get; set; }
    }
}
