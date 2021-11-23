using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Props.Settings;
using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential
{
    internal class AvatarGrainInitializable : IGrainInitializable
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IGameSettingManager _gameSettingManager;

        public AvatarGrainInitializable(IGrainFactory grainFactory,IGameSettingManager gameSettingManager)
        {
            _grainFactory = grainFactory;
            _gameSettingManager = gameSettingManager;
        }
        public async ValueTask InitializeAsync()
        {
            var ava =  _grainFactory.GetGrain<IAvatar>(1);
            var str = await ava.GetAvatarNameAsync();
            Console.WriteLine(str);
            var setting =await _gameSettingManager.GetAsync<PropsSetting>();
            Console.WriteLine(setting.Count);
        }
    }
}
