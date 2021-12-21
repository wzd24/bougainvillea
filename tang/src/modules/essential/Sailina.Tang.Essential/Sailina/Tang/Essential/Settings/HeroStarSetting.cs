using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 门客星级配置
    /// </summary>
    public class HeroStarSetting:GameSettingBase
    {

        /// <summary>
        /// 称号
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 升到此级消耗
        /// </summary>
        public string Depletion { get; set; }

        /// <summary>
        /// 升级后效果
        /// </summary>
        public string Effect { get; set; }

    }
}
