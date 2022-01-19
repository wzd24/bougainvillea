using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 插件对象描述定义接口
    /// </summary>
    public interface IPluginDescriptor
    {
        /// <summary>
        /// 判断插件描述对象中是否有对应的插件代码。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool ShouldBeCode(string code);

        /// <summary>
        /// 创建插件实例。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        IManagementPlugin Generate(string code,IServiceProvider serviceProvider);

        /// <summary>
        /// 获取所有插件描述
        /// </summary>
        public IEnumerable<PluginDescriptor> Descriptors { get; }

    }
}
