using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 门客突破配置
    /// </summary>
    public class HeroStudySetting:GameSettingBase
    {

        /// <summary>
        /// 升到此级消耗
        /// </summary>
        public string Depletion { get; set; }

        /// <summary>
        /// 突破所需名士等级
        /// </summary>
        public int RequireLevel { get; set; }

        /// <summary>
        /// 升级后效果
        /// </summary>
        public string Effect { get; set; }
    }
}
