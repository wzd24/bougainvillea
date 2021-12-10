using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Sailina.Tang.Essential.Avatars;
using Sailina.Tang.Essential.Dtos.Heros;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Depletion;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential.HeroSystem
{
    internal class HeroSubSystem : ISubSystem, ITransientDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private readonly IDepleteHandleManager _depleteHandleManager;
        private Avatar _avatar;
        private HeroState _data;

        public HeroSubSystem(IGameSettingManager gameSettingManager,IDepleteHandleManager depleteHandleManager)
        {
            _gameSettingManager = gameSettingManager;
            _depleteHandleManager = depleteHandleManager;
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
                var hero = new Hero(_avatar.Id, setting);
                _data.Add(heroId, hero);
            }
            else
            {
                var code = await _avatar.PropsSubSystem.PropsHandleManager.AddPropAsync(setting.StarItem, setting.Chip, reason);
                return code;
            }
            await _avatar.AddHead(heroId, reason);
            await _avatar.EventBus.PublishAsync(new HeroAddEventData(heroId, 1, reason));
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroId"></param>
        /// <returns></returns>
        public async ValueTask<HeroInfo> Get(int heroId)
        {
            var setting = (await _gameSettingManager.GetAsync<HeroSetting>()).SingleOrDefault(s => s.Id == heroId);
            if (setting == null)
            {
                return null;
            }
            var hero = _data.GetOrDefault(heroId);
            if (hero == null)
            {
                return null;
            }
            var info = hero.ToHeroInfo(setting);
            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroId"></param>
        /// <param name="lv"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async ValueTask<int> UpdateLv(int heroId,int lv,string reason)
        {
            var hero= _data.GetOrDefault(heroId);
            if (hero==null)
            {
                return 0;
            }

        }
    }
}
