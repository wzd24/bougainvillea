using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.Essential;

namespace Scorpio.Bougainvillea
{
    internal class GameGrainInitializable : IGrainInitializable
    {
        private readonly IGrainFactory _grainFactory;

        public GameGrainInitializable(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }
        public async ValueTask InitializeAsync()
        {
            await _grainFactory.GetGrain<IGame>(Guid.Empty).BeginInitializeAsync();
        }
    }
}
