using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    internal class CurrentServer : ICurrentServer, ISingletonDependency
    {
        private readonly IGameContextAccessor _accessor;

        public CurrentServer(IGameContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private User User => _accessor?.GameContext?.User as User;

        public int ServerId => User?.ServerId ?? 0;

    }
}
