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
    internal class CurrentUser : ICurrentUser, ISingletonDependency
    {

        public CurrentUser()
        {
        }

        public IGameUser User
        {
            get => RequestContext.Get("CurrentUser") as IGameUser;
            set => RequestContext.Set("CurrentUser", value);
        }

        public string Token => User?.Token;

        public int UserId => User?.UserId ?? 0;

        public int ServerId => User?.ServerId ?? 0;

        public int AvatarId => User.Id;

        public bool IsAuthentication => User != null;

        public IDisposable Use(IGameUser user)
        {
            var current = User;
            User = user;
            return new DisposeAction(() => User = current);
        }
    }
}
