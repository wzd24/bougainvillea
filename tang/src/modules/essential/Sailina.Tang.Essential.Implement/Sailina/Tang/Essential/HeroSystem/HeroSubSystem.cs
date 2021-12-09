using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sailina.Tang.Essential.Avatars;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential.HeroSystem
{
    internal class HeroSubSystem : ISubSystem, ITransientDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private Avatar _avatar;
        private HeroState _data;

        public HeroSubSystem(IGameSettingManager gameSettingManager)
        {
            _gameSettingManager = gameSettingManager;
        }
        public ValueTask InitializeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask OnSetupAsync(IAvatarBase avatarBase)
        {
            _avatar = avatarBase as Avatar;
            _data = _avatar.State.Heros;
            return ValueTask.CompletedTask;
        }

        public async ValueTask<int> AddHero(int heroId, string reason)
        {
            var setting = (await _gameSettingManager.GetAsync<HeroSetting>()).SingleOrDefault(s => s.Id == heroId);
            if (setting == null)
            {
                return 100002;
            }
            if (!_data.ContainsKey(heroId))
            {
                var hero = new HeroInfo
                {

                };
                _data.Add(heroId, hero);
            }
            await _avatar.EventBus.PublishAsync(new HeroAddEventData(heroId, 1, reason));
            return 0;
        }

    }
}
