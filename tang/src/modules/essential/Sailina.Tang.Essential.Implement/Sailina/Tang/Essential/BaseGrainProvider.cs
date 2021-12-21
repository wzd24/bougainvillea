using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Scorpio.Bougainvillea.Essential;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential
{
    internal class BaseGrainProvider : IBaseGrainProvider,ISingletonDependency
    {
        private readonly IGrainFactory _grainFactory;

        public BaseGrainProvider(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }
        public IAvatarBase GetAvatarBase(long avatarId) => _grainFactory.GetGrain<IAvatar>(avatarId);
        public IGameBase GetGameBase() => _grainFactory.GetGrain<IGame>(Guid.Empty);
        public IServerBase GetServerBase(int serverId) => _grainFactory.GetGrain<IServer>(serverId);
    }
}
