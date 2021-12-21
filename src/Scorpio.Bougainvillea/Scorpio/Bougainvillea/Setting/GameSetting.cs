using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 游戏设置项
    /// </summary>
    public class GameSetting:GameSettingBase
    {
        /// <summary>
        /// 区服Id
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 设置项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置项显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 设置项值
        /// </summary>
        public string Value { get; set; }
    }
}
