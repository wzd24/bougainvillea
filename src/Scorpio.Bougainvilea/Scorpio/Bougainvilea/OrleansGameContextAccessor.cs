using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans.Runtime;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvilea
{
    internal class OrleansGameContextAccessor : IGameContextAccessor, ISingletonDependency
    {

        public IGameContext GameContext
        {
            get => RequestContext.Get("GameContext") as IGameContext;
            set => RequestContext.Set("GameContext", value);
        }
    }
}
