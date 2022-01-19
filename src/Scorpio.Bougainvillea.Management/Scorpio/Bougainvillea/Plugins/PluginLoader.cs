using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class PluginLoader : IPluginLoader, ISingletonDependency
    {
        private readonly PluginLoadOptions _options;

        public PluginLoader(IOptions<PluginLoadOptions> options)
        {
            _options = options.Value;
        }

        public int Order { get; } = 1;
        public IEnumerable<PluginDescriptor>  Descriptors => _options.Plugins.SelectMany(p => p.Descriptors);

        public IManagementPlugin GetPlugin(string code, IServiceProvider serviceProvider)
        {
            var descriptor = GetPluginDescriptor(code);
            if (descriptor == null)
            {
                return null;
            }
            var plug = descriptor.Generate(code, serviceProvider);
            return plug;
        }

        private IPluginDescriptor GetPluginDescriptor(string code) => _options.Plugins.FirstOrDefault(p => p.ShouldBeCode(code));
    }
}
