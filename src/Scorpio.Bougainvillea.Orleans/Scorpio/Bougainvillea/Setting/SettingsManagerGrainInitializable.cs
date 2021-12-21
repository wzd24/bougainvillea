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
        private readonly IGameSettingDefinitionManager _gameSettingDefinitionManager;

        public SettingsManagerGrainInitializable(IGrainFactory grainFactory, IGameSettingDefinitionManager gameSettingDefinitionManager)
        {
            _grainFactory = grainFactory;
            _gameSettingDefinitionManager = gameSettingDefinitionManager;
        }
        public async ValueTask InitializeAsync()
        {
            await _gameSettingDefinitionManager.GetAll().Where(x => x.Scope == GameSettingScope.Global).ForEachAsync(async x => await _grainFactory.GetGrain<IGlobalSettingManager>(x.Name).InitializeAsync());
        }
    }
}
