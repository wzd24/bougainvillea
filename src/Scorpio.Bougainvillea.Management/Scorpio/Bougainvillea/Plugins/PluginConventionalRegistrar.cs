using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class PluginConventionalRegistrar : IConventionalRegistrar
    {
        public void Register(IConventionalRegistrationContext context) => 
            context.DoConventionalAction<PluginConventionalAction>(
                c => c.Where(t => t.IsStandardType())
                      .Where(t => t.AttributeExists<PluginCodeAttribute>()));
    }
}
