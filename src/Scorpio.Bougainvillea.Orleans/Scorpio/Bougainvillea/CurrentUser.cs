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

        public long AvatarId
        {
            get => RequestContext.Get("CurrentUser")?.To<long>()??0;
            set => RequestContext.Set("CurrentUser", value);
        }

   
        public IDisposable Use(long avatarId)
        {
            var current = AvatarId;
            AvatarId = avatarId;
            return new DisposeAction(() => AvatarId = current);
        }
    }
}
