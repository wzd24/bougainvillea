using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ConventionalGameSettingDefinitionProviderRegisterAction : ConventionalActionBase
    {
        public ConventionalGameSettingDefinitionProviderRegisterAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t =>
            {
                context.Services.AddSingleton(t);
                context.Services.Configure<GameSettingOptions>(opt => opt.DefinitionProviders.AddIfNotContains(t));
            });
        }
    }
}
