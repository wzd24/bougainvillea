using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;


using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class PluginManager : IPluginManager, ISingletonDependency
    {
        private readonly IEnumerable<IPluginLoader> _pluginLoaders;

        public PluginManager(IEnumerable<IPluginLoader> pluginLoaders)
        {
            _pluginLoaders = pluginLoaders.OrderBy(p => p.Order);
        }

        public IManagementPlugin GetPlugin(IServiceProvider serviceProvider, string code)
        {
            IManagementPlugin plugin = null;
            foreach (var item in _pluginLoaders)
            {
                plugin = item.GetPlugin(code, serviceProvider);
                if (plugin != null)
                {
                    break;
                }
            }
            return plugin ?? throw new GameFriendlyException(-1, "对应的插件不存在");
        }
    }
}
