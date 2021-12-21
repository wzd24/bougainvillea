using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 门客等级(此配置只支持100级以后，之后根据算法推算)
    /// </summary>
    [Serializable]
    public class HeroLevelSetting:GameSettingBase
    {

        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 升到此级消耗阅历
        /// </summary>
        public string Depletion { get; set; }

    }
}
