using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 皮肤配置
    /// </summary>
    public class SkinSetting:GameSettingBase
    {
        /// <summary>
        /// 皮肤类型（1门客皮肤，2美女皮肤）
        /// </summary>
        public short SkinType { get; set; }

        /// <summary>
        /// 皮肤所属ID(SkinType为1，则为门客ID；SkinType为2，则为红颜ID)
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName { get; set; }

        /// <summary>
        /// 皮肤描述
        /// </summary>
        public string SkinDes { get; set; }

        /// <summary>
        /// 皮肤称号
        /// </summary>
        public int Title { get; set; }

        /// <summary>
        /// 皮肤品质
        /// </summary>
        public Quality Quality { get; set; }

    }
}
