using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Depletion;
using Scorpio.Bougainvillea.Rewards;
using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ConventionalAttributeDepleteHandlerRegisterAction : ConventionalActionBase
    {
        public ConventionalAttributeDepleteHandlerRegisterAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t =>
            {
                context.Services.Configure<AttributeDepleteHandlerOptions>(opt => opt.AddHandlerType(t));
            });
        }
    }
}
