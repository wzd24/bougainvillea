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
        private readonly ICurrentUser _currentUser;

        public CurrentServer(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public int ServerId =>_currentUser.ServerId;

    }
}
