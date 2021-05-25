using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Scorpio.Bougainvillea.Props.Settings
{

    /// <summary>
    /// 道具配置
    /// </summary>
    [Serializable]
    public sealed class PropsSetting:GameSettingBase
    {
        /// <summary>
        /// 道具排序Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 道具是否显示在背包中
        /// </summary>
        public Visible Visible { get; set; }

        /// <summary>
        /// 道具名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 道具类型
        /// </summary>
        public int PropType { get; set; }

        /// <summary>
        /// 道具描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 道具使用类型
        /// </summary>
        public UseType UseType { get; set; }

        /// <summary>
        /// 使用效果，效果参数配置 如: { }
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// 道具过期时长，-1为永不过期。
        /// </summary>
        public int ExpiredSeconds { get; set; }

        /// <summary>
        /// 批量使用上限(1:不能批量使用;0:无限)
        /// </summary>
        public int BatchUseLimit { get; set; }

    }

    /// <summary>
    /// 是否显示
    /// </summary>
    public enum Visible
    {
        /// <summary>
        /// 不显示
        /// </summary>
        No = 0,
        /// <summary>
        /// 显示
        /// </summary>
        Yes = 1
    }

    /// <summary>
    /// 道具使用类型
    /// </summary>
    public enum UseType
    {
        /// <summary>
        /// 不可使用
        /// </summary>
        CanNotUse = -2,
        /// <summary>
        /// 不可直接使用
        /// </summary>
        CanNotBeUsedDirectly = 1,
        /// <summary>
        /// 获取道具直接使用
        /// </summary>
        GetPropsAndUseThemDirectly = 2,
        /// <summary>
        /// 背包内使用
        /// </summary>
        UseInTheBackPack = 3
    }
}
