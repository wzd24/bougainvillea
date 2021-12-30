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
    /// 美名-基础配置表
    /// </summary>
    public class BeautyTitleSetting:GameSettingBase
    {
        /// <summary>
        /// 美名名称(女性)
        /// </summary>
        public string FemaleName { get; set; }

        /// <summary>
        /// 美名名称(男性)
        /// </summary>
        public string MaleName { get; set; }

        /// <summary>
        /// 升级条件
        /// </summary>
        public List<int> UpgradeCondition { get; set; }
    }
}
