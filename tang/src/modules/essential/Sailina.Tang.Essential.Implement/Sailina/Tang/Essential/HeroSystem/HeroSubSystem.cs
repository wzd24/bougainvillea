using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Orleans;

using Sailina.Tang.Essential.Avatars;
using Sailina.Tang.Essential.Dtos.Heros;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Depletion;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea.Skills.Settings;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential.HeroSystem
{
    internal class HeroSubSystem : ISubSystem, ITransientDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private readonly IDepleteHandleManager _depleteHandleManager;
        private readonly IJsonSerializer _jsonSerializer;
        private Avatar _avatar;
        private HeroState _data;

        public HeroSubSystem(IGameSettingManager gameSettingManager, IDepleteHandleManager depleteHandleManager, IJsonSerializer jsonSerializer)
        {
            _gameSettingManager = gameSettingManager;
            _depleteHandleManager = depleteHandleManager;
            _jsonSerializer = jsonSerializer;
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

        public async ValueTask<int> UnlockHero(int heroId, string reason)
        {
            var setting = (await _gameSettingManager.GetAsync<HeroSetting>()).SingleOrDefault(s => s.Id == heroId);
            if (setting == null)
            {
                return (int)CommonErrorCode.ParaError;
            }
            if (!_data.ContainsKey(heroId))
            {
                var hero = new Hero(_avatar.Id, setting);
                var skillSettings = (await _gameSettingManager.GetAsync<SkillSetting>()).Where(s=>s.OwnerType==1 && s.OwnerId==heroId && s.UnLockCondition.IsNullOrWhiteSpace());
                skillSettings.ForEach(s =>
                {
                    hero.Skills.AddOrUpdate(s.Id,k=> 1);
                });
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
            var setting = await _gameSettingManager.GetAsync<HeroSetting>(heroId);
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
        public async ValueTask<int> UpgradeLv(int heroId, int lv, string reason)
        {
            var hero = _data.GetOrDefault(heroId);
            if (hero == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var maxId = await _gameSettingManager.GetMaxIdAsync<HeroLevelSetting>();
            if (maxId <= hero.Lv)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var oldLv = hero.Lv;
            for (var i = 0; i < lv; i++)
            {
                var newLv = hero.Lv + 1;
                var setting = await _gameSettingManager.GetAsync<HeroLevelSetting>(newLv);
                if (setting == null)
                {
                    break;
                }
                var code = await _depleteHandleManager.CanHandleAsync(setting.Depletion, 1);
                if (code != 0)
                {
                    return code;
                }
                code = await _depleteHandleManager.HandleAsync(setting.Depletion, 1, reason);
                if (code != 0)
                {
                    break;
                }
                hero.Lv = newLv;
            }
            if (hero.Lv > oldLv)
            {
                await _avatar.EventBus.PublishAsync(new HeroUpgradeEventData(heroId, hero.Lv - oldLv, hero.Lv, reason));
            }
            return 0;
        }


        public async ValueTask<int> UpgradeHeroStudy(int heroId, string reason)
        {
            var hero = _data.GetOrDefault(heroId);
            if (hero == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var maxId = await _gameSettingManager.GetMaxIdAsync<HeroStudySetting>();
            if (maxId <= hero.StudyLv)
                return (int)CommonErrorCode.AlreadyMax;
            var setting = await _gameSettingManager.GetAsync<HeroStudySetting>(hero.StudyLv + 1);
            if (hero.Lv < setting.RequireLevel)
                return HeroErrorCode.NotEnoughtStudyLevel;
            var code = await _depleteHandleManager.CanHandleAsync(setting.Depletion, 1);
            if (code != 0)
            {
                return code;
            }
            code = await _depleteHandleManager.HandleAsync(setting.Depletion, 1, reason);
            if (code != 0)
            {
                return code;
            }
            hero.StudyLv += 1;
            await _avatar.EventBus.PublishAsync(new HeroStudyUpgradeEventData(heroId, 1, hero.StudyLv, reason));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> UpgradeHeroStar(int heroId, string reason)
        {
            var hero = _data.GetOrDefault(heroId);
            if (hero == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var maxId = await _gameSettingManager.GetMaxIdAsync<HeroStarSetting>();
            if (maxId <= hero.StarLv)
                return (int)CommonErrorCode.AlreadyMax;
            var setting = await _gameSettingManager.GetAsync<HeroStarSetting>(hero.StarLv + 1);
            var depletions = _jsonSerializer.Deserialize<JToken[][]>(setting.Depletion).ToDictionary(t => t[0].Value<Quality>(), t => t[1].ToString(Newtonsoft.Json.Formatting.None));
            var heroSetting = await _gameSettingManager.GetAsync<HeroSetting>(heroId);
            var depletion = depletions.GetOrDefault(heroSetting.Quality);
            if (depletion == null)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var code = await _depleteHandleManager.CanHandleAsync(depletion, 1);
            if (code != 0)
                return code;
            code = await _depleteHandleManager.HandleAsync(depletion, 1, reason);
            if (code != 0)
                return code;
            hero.StarLv += 1;
            await _avatar.EventBus.PublishAsync(new HeroStarUpgradeEventData(heroId, 1, hero.StudyLv, reason));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> UpgradeHeroSkill(int heroId, int skillId, string reason)
        {
            var hero = _data.GetOrDefault(heroId);
            if (hero == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var setting = await _gameSettingManager.GetAsync<SkillSetting>(skillId);
            if (setting == null)
            {
                return HeroErrorCode.NotExistSkill;
            }
            if (setting.OwnerType != 1 || setting.OwnerId != heroId)
            {
                return (int)CommonErrorCode.ParaError;
            }
            var level = hero.Skills.GetOrDefault(skillId, 0);
            if (level == 0)
                return HeroErrorCode.NotUnlockSkill;
            if (level >= setting.MaxLv)
                return (int)CommonErrorCode.AlreadyMax;
            var code = await _depleteHandleManager.CanHandleAsync(setting.Depletion, level);
            if (code != 0)
                return code;
            code = await _depleteHandleManager.HandleAsync(setting.Depletion, level, reason);
            if (code != 0)
                return code;
            hero.Skills.AddOrUpdate(skillId, k => 1, (k, v) => v + 1);
            await _avatar.EventBus.PublishAsync(new HeroSkillUpgradeEventData(heroId,skillId, 1, hero.Skills[skillId], reason));
            return SystemErrorCodes.Success;
        }

    }
}
