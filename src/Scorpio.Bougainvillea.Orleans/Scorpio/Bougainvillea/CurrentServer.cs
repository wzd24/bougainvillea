using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans.Runtime;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    internal class CurrentServer : ICurrentServer, ISingletonDependency
    {

        public CurrentServer()
        {
        }

        public int ServerId
        {
            get => RequestContext.Get("CurrentServer")?.To<int>()??0;
            set => RequestContext.Set("CurrentServer", value);
        }

        public IDisposable Use(int serverId)
        {
            var current = ServerId;
            ServerId = serverId;
            return new DisposeAction(() => ServerId = current);
        }
    }
}
