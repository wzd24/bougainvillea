using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{

    /// <summary>
    /// 子嗣形象表
    /// </summary>
    [Serializable]
    public class KidImageSetting:GameSettingBase
    {
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 职业限定
        /// </summary>
        public int Job { get; set; }
    }
}
