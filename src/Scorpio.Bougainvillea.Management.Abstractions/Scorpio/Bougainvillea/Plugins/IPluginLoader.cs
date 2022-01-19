using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPluginLoader
    {
        /// <summary>
        /// 
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        IManagementPlugin GetPlugin(string code,IServiceProvider serviceProvider);

        /// <summary>
        /// 获取所有插件描述
        /// </summary>
        public IEnumerable<PluginDescriptor> Descriptors { get; }
    }
}
