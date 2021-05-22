using Microsoft.Extensions.DependencyInjection;
using Scorpio.Conventional;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Handler
{
    class ConventionalHandlersRegisterAction : ConventionalActionBase
    {

        public ConventionalHandlersRegisterAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t => context.Services.Configure<GameHandlerOptions>(opt => opt.AddType(t)));
        }

    }
}
