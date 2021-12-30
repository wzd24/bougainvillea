using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sailina.Tang.Essential.Settings
{
    /// <summary>
    /// 品质
    /// </summary>
    public enum Quality
    {
        /// <summary>
        /// 白
        /// </summary>
        White = 1,
        /// <summary>
        /// 绿
        /// </summary>
        Green = 2,
        /// <summary>
        /// 蓝
        /// </summary>
        Bule = 3,
        /// <summary>
        /// 紫
        /// </summary>
        Purple = 4,
        /// <summary>
        /// 橙
        /// </summary>
        Orange = 5,
        /// <summary>
        /// 红
        /// </summary>
        Red = 6
    }

    /// <summary>
    /// 子嗣品质
    /// </summary>
    public enum KidQuality
    {
        /// <summary>
        /// 平庸
        /// </summary>
        [Display(Name = "平庸")]
        PingYong = 1,
        /// <summary>
        /// 活泼
        /// </summary>
        [Display(Name = "活泼")]
        Huopo = 2,
        /// <summary>
        /// 伶俐
        /// </summary>
        [Display(Name = "伶俐")]
        LingLi = 3,
        /// <summary>
        /// 机敏
        /// </summary>
        [Display(Name = "机敏")]
        JiMin = 4,
        /// <summary>
        /// 聪明
        /// </summary>
        [Display(Name = "聪明")]
        CongMing = 5,
        /// <summary>
        /// 颖慧
        /// </summary>
        [Display(Name = "颖慧")]
        YinHui = 6,
        /// <summary>
        /// 睿智
        /// </summary>
        [Display(Name = "睿智")]
        RuiZhi = 7,
        /// <summary>
        /// 天才
        /// </summary>
        [Display(Name = "天才")]
        TianCai = 8,
        /// <summary>
        /// 神童
        /// </summary>
        [Display(Name = "神童")]
        ShenTong = 9,
    }
    /// <summary>
    /// 子嗣授学类型
    /// </summary>
    public enum TeachingType
    {
        /// <summary>
        /// 未授学
        /// </summary>
        NotTeaching = 0,
        /// <summary>
        /// 自学
        /// </summary>
        SelfStudy = 1,
        /// <summary>
        /// 私塾
        /// </summary>
        PersonalTrainer = 2,
        /// <summary>
        /// 学府
        /// </summary>
        School = 3,
        /// <summary>
        /// 名师
        /// </summary>
        FamousTeacher = 4,
        /// <summary>
        /// 太学
        /// </summary>
        TheImperialCollege = 5,
        /// <summary>
        /// 大师
        /// </summary>
        GreatMaster = 6,
        /// <summary>
        /// 巨擘
        /// </summary>
        Giant = 7,
        /// <summary>
        /// 鼻祖
        /// </summary>
        Founder = 8,
    }
}
