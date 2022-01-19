using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class TypePluginDescriptor : IPluginDescriptor
    {
        private readonly PluginDescriptor _descriptor;
        public TypePluginDescriptor(Type type)
        {
            _descriptor = new PluginDescriptor(type);
        }

        public IEnumerable<PluginDescriptor> Descriptors
        {
            get { yield return _descriptor; }
        }

        public IManagementPlugin Generate(string code, IServiceProvider serviceProvider) =>
            _descriptor.Generate(serviceProvider);
        public bool ShouldBeCode(string code) => code == _descriptor.Code;
    }
}
