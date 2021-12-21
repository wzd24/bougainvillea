using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RoleSetting:Setting.GameSettingBase
    {
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 默认形象
        /// </summary>
        public int Image { get; set; }

        /// <summary>
        /// 默认头像框
        /// </summary>
        public int HeadFrameId { get; set; }

        /// <summary>
        /// 可用头像列表
        /// </summary>
        public List<int> Heads { get; set; }
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 未指定
        /// </summary>
        Unspecified,
        /// <summary>
        /// 男
        /// </summary>
        Male,

        /// <summary>
        /// 女
        /// </summary>
        Female
    }
}
