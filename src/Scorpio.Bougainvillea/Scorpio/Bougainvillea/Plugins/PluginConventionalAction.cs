using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Plugins
{
    internal class PluginConventionalAction : ConventionalActionBase
    {
        public PluginConventionalAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context) => 
            context.Services.Configure<PluginLoadOptions>(
                opts => context.Types.ForEach(t => opts.AddType(t)));
    }
}
