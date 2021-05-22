using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    internal class CurrentUser : ICurrentUser, ISingletonDependency
    {
        private readonly IGameContextAccessor _accessor;

        public CurrentUser(IGameContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private User User => _accessor?.GameContext?.User as User;

        public string Token => User?.Token;

        public int UserId => User?.UserId ?? 0;

        public int ServerId => User?.ServerId ?? 0;

        public int Id => User.Id;

        public bool IsAuthentication => User != null;
    }
}
