using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Sailina.Tang.Essential.HeroSystem;
using Sailina.Tang.Essential.Settings;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Depletion;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea.Skills;
using Scorpio.Bougainvillea.Skills.Settings;
using Scorpio.DependencyInjection;
using Scorpio.EventBus;
using Scorpio.Setting;

namespace Sailina.Tang.Essential.BeautySystem
{
    internal class BeautySubSystem : ISubSystem, ITransientDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private readonly ISettingManager _settingManager;
        private readonly IDepleteHandleManager _depleteHandleManager;
        private readonly IJsonSerializer _jsonSerializer;
        private Avatar _avatar;
        private BeautyState _data;

        public BeautySubSystem(IGameSettingManager gameSettingManager, ISettingManager settingManager, IDepleteHandleManager depleteHandleManager, IJsonSerializer jsonSerializer)
        {
            _gameSettingManager = gameSettingManager;
            _settingManager = settingManager;
            _depleteHandleManager = depleteHandleManager;
            _jsonSerializer = jsonSerializer;
        }
        public ValueTask InitializeAsync() => ValueTask.CompletedTask;
        public ValueTask OnSetupAsync(IAvatarBase avatarBase)
        {
            _avatar = avatarBase as Avatar;
            _data = _avatar.State.Beauties;
            return ValueTask.CompletedTask;
        }

        public async ValueTask<int> UnLock(int beautyId, string reason)
        {
            var setting = (await _gameSettingManager.GetAsync<BeautySetting>()).SingleOrDefault(s => s.Id == beautyId);
            if (setting == null)
            {
                return (int)CommonErrorCode.ParaError;
            }
            if (!_data.ContainsKey(beautyId))
            {
                var hero = new Beauty(_avatar.Id, setting);
                _data.Add(beautyId, hero);
            }
            else
            {
                var code = await _avatar.PropsSubSystem.PropsHandleManager.AddPropAsync(setting.StarItem, setting.Chip, reason);
                return code;
            }
            await _avatar.AddHead(beautyId, reason);
            await _avatar.EventBus.PublishAsync(new BeautyAddEventData(beautyId, reason));
            return 0;
        }

        public async ValueTask<int> UpgradeTitleLV(int beautyId, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return (int)CommonErrorCode.NotUnlock;
            }
            var maxId = await _gameSettingManager.GetMaxIdAsync<BeautyTitleSetting>();
            if (maxId <= beauty.TitleLv)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var setting = await _gameSettingManager.GetAsync<BeautyTitleSetting>(beauty.TitleLv + 1);
            if (maxId <= beauty.TitleLv)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var (vQinMi, vMeiLi, vTotalBeauties) = (setting.UpgradeCondition[0], setting.UpgradeCondition[1], setting.UpgradeCondition[2]);
            if (beauty.TotalQinMi < vQinMi || beauty.TotalMeiLi < vMeiLi || _data.Count < vTotalBeauties)
            {
                return BeautyErrorCode.UpgradeTitleFail4CanNot;
            }
            beauty.TitleLv += 1;
            beauty.ShopSkills.Keys.ToArray().ForEach(skill => beauty.ShopSkills.AddOrUpdate(skill, k => 1, (k, v) => v + 1));
            //发布美名提升事件
            await _avatar.EventBus.PublishAsync(new BeautyTitleUpgradeEventData(beautyId, reason, 1, beauty.TitleLv));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> UpgradeStarLV(int beautyId, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return (int)CommonErrorCode.NotUnlock;
            }
            var maxId = await _gameSettingManager.GetMaxIdAsync<BeautyStarSetting>();
            if (maxId <= beauty.TitleLv)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var setting = await _gameSettingManager.GetAsync<BeautyStarSetting>(beauty.StarLv + 1);
            if (maxId <= beauty.TitleLv)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var code = await _depleteHandleManager.CanHandleAsync(setting.Depletion, 1);
            if (code != 0)
                return code;
            code = await _depleteHandleManager.HandleAsync(setting.Depletion, 1, reason);
            if (code != 0)
                return code;
            beauty.StarLv += 1;
            //发布美名提升事件
            await _avatar.EventBus.PublishAsync(new BeautyStarUpgradeEventData(beautyId, reason, 1, beauty.StarLv));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> UnlockOrUpgradeSkin(int beautyId, int skinId, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var skinSetting = await _gameSettingManager.GetAsync<SkinSetting>(skinId);
            if (skinSetting == null || skinSetting.SkinType != 2 || skinSetting.OwnerId != beautyId)
            {
                return (int)CommonErrorCode.Skin_UnlockOrUpgradeFail;
            }
            var level = beauty.Skins.GetOrDefault(skinId, 0) + 1;
            var levelSetting = await _gameSettingManager.GetAsync<SkinLevelSetting>(skinId * 100 + level);
            if (levelSetting == null)
            {
                return (int)CommonErrorCode.AlreadyMax;
            }
            var code = await _depleteHandleManager.CanHandleAsync(levelSetting.Depletion, 1);
            if (code != SystemErrorCodes.Success)
            {
                return code;
            }
            code = await _depleteHandleManager.HandleAsync(levelSetting.Depletion, 1, reason);
            if (code != SystemErrorCodes.Success)
            {
                return code;
            }
            beauty.Skins.AddOrUpdate(skinId, k => 1, (k, v) => v + 1);
            await _avatar.EventBus.PublishAsync(new BeautySkinUpgradeEventData(beautyId, skinId, reason, 1, beauty.Skins[skinId]));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> WearSkin(int beautyId, int skinId, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            if (!beauty.Skins.ContainsKey(skinId))
            {
                return (int)CommonErrorCode.Skin_WearFail;
            }
            var oldId = beauty.WearSkinId;
            beauty.WearSkinId = skinId;
            await _avatar.EventBus.PublishAsync(new BeautySkinWearEventData(beautyId, reason, oldId, skinId));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> UpgradeFateSkill(int beautyId, int skillId, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return HeroErrorCode.UnlockedHero;
            }
            var setting = await _gameSettingManager.GetAsync<SkillSetting>(skillId);
            if (setting == null)
            {
                return HeroErrorCode.NotExistSkill;
            }
            if (setting.OwnerType != 1 || setting.OwnerId != beautyId)
            {
                return (int)CommonErrorCode.ParaError;
            }
            var level = beauty.FateSkills.GetOrDefault(skillId, 0);
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
            beauty.FateSkills.AddOrUpdate(skillId, k => 1, (k, v) => v + 1);
            await _avatar.EventBus.PublishAsync(new BeautySkillUpgradeEventData(beautyId, reason, skillId, 1, beauty.FateSkills[skillId]));
            return SystemErrorCodes.Success;
        }

        public async ValueTask<int> Presenting(int beautyId, int propId, int num, string reason)
        {
            var beauty = _data.GetOrDefault(beautyId);
            if (beauty == null)
            {
                return (int)CommonErrorCode.NotUnlock;
            }
            var setting = await _settingManager.GetAsync<List<int>>("BeautyGiftList");
            if (setting.Contains(propId) == false)
                return (int)CommonErrorCode.ParaError;//未知道具
            if (await _avatar.PropsSubSystem.PropsHandleManager.EnoughAsync(propId, num) != 0)
            {
                return BeautyErrorCode.GivingFail;
            }
            var (code, _) = await _avatar.PropsSubSystem.PropsHandleManager.UseAsync(propId, num, reason, beautyId);
            if (code == 0)
            {
                await _avatar.EventBus.PublishAsync(new BeautyPresentingEventData(beautyId, reason, propId, num));
            }
            return code;
        }
    }
}
