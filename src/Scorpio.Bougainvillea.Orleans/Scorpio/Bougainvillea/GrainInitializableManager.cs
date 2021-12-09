using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    internal class GrainInitializableManager : IGrainInitializableManager, ISingletonDependency
    {
        private readonly IEnumerable<IGrainInitializable> _initializables;

        public GrainInitializableManager(IEnumerable<IGrainInitializable> initializables)
        {
            _initializables = initializables;
        }
        public async ValueTask InitializeAsync()
        {
            await _initializables.ForEachAsync(async i => await i.InitializeAsync());
        }
    }
}
