using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime;

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
        private readonly IJsonSerializer _jsonSerializer;

        public AvatarGrainInitializable(IGrainFactory grainFactory,IGameSettingManager gameSettingManager,IJsonSerializer jsonSerializer)
        {
            _grainFactory = grainFactory;
            _gameSettingManager = gameSettingManager;
            _jsonSerializer = jsonSerializer;
        }
        public async ValueTask InitializeAsync()
        {
            var server =  _grainFactory.GetGrain<IServer>(1);
            var ava = await server.GetAvatarAsync(1000001);
            var  avatarProps = _grainFactory.GetGrain<IAvatarProps>(ava.AvatarId);
            await avatarProps.AddPropAsync(1201, 10, "ceshi");
            await avatarProps.UseAsync(1201, 1, "ceshi");
            var avatar = _grainFactory.GetGrain<IAvatar>(ava.AvatarId);
            await avatar.SaveAsync();
        }
    }
}
