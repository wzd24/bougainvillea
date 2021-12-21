using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Rewards;
using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ConventionalAttributeRewardHandlerRegisterAction : ConventionalActionBase
    {
        public ConventionalAttributeRewardHandlerRegisterAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t =>
            {
                context.Services.AddTransient(t);
                context.Services.Configure<AttributeRewardHandlerOptions>(opt => opt.AddHandlerType(t));
            });
        }
    }
}
