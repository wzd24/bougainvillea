using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 游戏服管理插件定义接口
    /// </summary>
    public interface IManagementPlugin:IDisposable
    {
        /// <summary>
        /// 根据指定的上下文执行插件。
        /// </summary>
        /// <param name="context">插件执行上下文</param>
        /// <returns>插件执行返回值</returns>
        Task<object> ExecuteAsync(ManagementPluginExecutionContext context);

    }
}
