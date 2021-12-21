using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Conventional;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ConventionalSettingDefinitionProviderRegisterAction : ConventionalActionBase
    {
        public ConventionalSettingDefinitionProviderRegisterAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t =>
            {
                context.Services.AddSingleton(t);
                context.Services.Configure<SettingOptions>(opt => opt.DefinitionProviders.AddIfNotContains(t));
            });
        }
    }
}
