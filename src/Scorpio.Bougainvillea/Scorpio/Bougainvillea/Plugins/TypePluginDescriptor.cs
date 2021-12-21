using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class TypePluginDescriptor : IPluginDescriptor
    {
        private readonly string _code;
        private readonly Type _type;
        public TypePluginDescriptor(Type type)
        {
            _type = type;
            _code = type.GetAttribute<PluginCodeAttribute>().Code;
        }
        public IManagementPlugin Generate(string code, IServiceProvider serviceProvider) =>
            ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, _type) as IManagementPlugin;
        public bool ShouldBeCode(string code) => code == _code;
    }
}
