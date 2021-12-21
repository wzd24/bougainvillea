using System.ComponentModel.DataAnnotations;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 职业
    /// </summary>
    public enum Profession
    {
        /// <summary>
        /// 未知职业
        /// </summary>
        [Display(Name = "未知")]

        Default = 0,
        /// <summary>
        /// 墨客
        /// </summary>
        [Display(Name = "墨客")]
        MoKe = 1,
        /// <summary>
        /// 修者
        /// </summary>
        [Display(Name = "修者")]
        XiuZhe = 2,
        /// <summary>
        /// 巧匠
        /// </summary>
        [Display(Name = "巧匠")]
        QiaoJiang = 3,
        /// <summary>
        /// 商贾
        /// </summary>
        [Display(Name = "商贾")]
        ShangGu = 4,
        /// <summary>
        /// 侠士
        /// </summary>
        [Display(Name = "侠士")]
        XiaShi = 5,
    }
}
