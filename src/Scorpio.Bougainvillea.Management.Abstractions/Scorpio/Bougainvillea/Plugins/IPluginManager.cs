using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPluginManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IManagementPlugin GetPlugin(IServiceProvider serviceProvider, string code);

        /// <summary>
        /// 获取所有插件描述
        /// </summary>
        public IEnumerable<PluginDescriptor> Descriptors { get; }

    }
}
