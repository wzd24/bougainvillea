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
    public class SkinLevelSetting:GameSettingBase
    {
        /// <summary>
        /// 皮肤ID
        /// </summary>
        public int SkinId { get; set; }

        /// <summary>
        /// 皮肤等级
        /// </summary>
        public int Level { get; set; }

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
