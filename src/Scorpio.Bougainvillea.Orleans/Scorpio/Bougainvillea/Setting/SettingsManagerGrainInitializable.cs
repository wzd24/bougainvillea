using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Setting
{
    internal class SettingsManagerGrainInitializable : IGrainInitializable
    {
        private readonly IGrainFactory _grainFactory;

        public SettingsManagerGrainInitializable(IGrainFactory grainFactory)
        {
            _grainFactory = grainFactory;
        }
        public async ValueTask InitializeAsync()
        {
            await _grainFactory.GetGrain<IGlobalSettingManager>(Guid.Empty).InitializeAsync();
        }
    }
}
