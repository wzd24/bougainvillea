using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;
using Orleans.CodeGeneration;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.Streams;
using Orleans.Streams.Core;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAvatar"></typeparam>
    /// <typeparam name="TAvatarState"></typeparam>
    /// <typeparam name="TEntityBase"></typeparam>
    [ImplicitStreamSubscription(AvatarBase.StreamSubscription)]
    public abstract class AvatarBase<TAvatar, TAvatarState, TEntityBase> : GrainBase<TAvatar>, IAvatarBase
         where TAvatar : AvatarBase<TAvatar, TAvatarState, TEntityBase>
         where TAvatarState : AvatarStateBase<TEntityBase>
        where TEntityBase:AvatarEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AvatarBaseStateStorageName = "AvatarBaseStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarBaseStateName = "AvatarBaseState";

        private StreamSubscriptionHandle<GenerateInfo> _handler;

        /// <summary>
        /// 
        /// </summary>
        protected ICurrentUser CurrentUser { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(AvatarBaseStateName, AvatarBaseStateStorageName)]
        protected IPersistentState<TAvatarState> AvatarState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected AvatarBase(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
            CurrentUser = serviceProvider.GetService<ICurrentUser>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnActivateAsync()
        {
            _handler = await this.GetStreamAsync<GenerateInfo>(this.GetPrimaryKey(), AvatarBase.StreamSubscription).SubscribeAsync(async (d, t) => await GenerateAsync(d));
            await base.OnActivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnDeactivateAsync()
        {
            await _handler?.UnsubscribeAsync();
            await base.OnDeactivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        public async virtual Task<int> GenerateAsync(GenerateInfo generateInfo)
        {
            var roleSetting=(await GameSettingManager.GetAsync<RoleSetting>()).FirstOrDefault(r=>r.Sex==generateInfo.Sex);
            if (roleSetting==null)
            {
                return (int)ErrorCode.ConfigurationDoesNotExist;
            }
            await InitAvatarBaseInfoAsync(generateInfo, roleSetting.HeadFrameId);
            await GenerateCoreAsync(generateInfo);
            return (int)ErrorCode.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        protected virtual Task<int> GenerateCoreAsync(GenerateInfo generateInfo)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <param name="headFrameId"></param>
        /// <returns></returns>
        protected virtual async Task InitAvatarBaseInfoAsync(GenerateInfo generateInfo, int headFrameId)
        {
            AvatarState.State.Base.Id = CurrentUser.AvatarId;
            AvatarState.State.Base.UserId = CurrentUser.UserId;
            AvatarState.State.Base.ServerId = CurrentUser.ServerId;
            AvatarState.State.Base.NickName = generateInfo.Name;
            AvatarState.State.Base.Image = generateInfo.Image;
            AvatarState.State.Base.Level = 1;
            AvatarState.State.Base.CreateDate = DateTime.Now;
            AvatarState.State.Base.Sex = generateInfo.Sex;
            AvatarState.State.Base.HeadId = generateInfo.HeadId;
            AvatarState.State.Base.HeadFrameId = headFrameId;
            await AvatarState.WriteStateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task PostGenerateAsync()
        {
            return Task.CompletedTask;
        }



        /// <summary>
        /// 
        /// </summary>
        private enum ErrorCode
        {
            /// <summary>
            /// 执行成功
            /// </summary>
            None = 0,
            /// <summary>
            /// 配置不存在
            /// </summary>
            ConfigurationDoesNotExist = 160000,
            /// <summary>
            /// 身份升级，未满足升级条件
            /// </summary>
            UpgradeConditionNotMet = 160001,
            /// <summary>
            /// 领取身份奖励，奖励已领取
            /// </summary>
            RewardHasBeenClaimed = 160002,
            /// <summary>
            /// 改名，输入字符禁止为空
            /// </summary>
            InputCharactersAreDisallowedToBeNull = 160003,
            /// <summary>
            /// 包含铭感词
            /// </summary>
            ContainsMaskingCharacters = 160015,
            /// <summary>
            /// 创角失败
            /// </summary>
            GenerateAvatarLose = 160016,
            /// <summary>
            /// 角色已存在
            /// </summary>
            CharactersAlreadyExist = 160023,
            /// <summary>
            /// 创角失败，头像不可选
            /// </summary>
            GenerateAvatarLoseHeadDoNotChoose = 160024,
            /// <summary>
            /// 改名与当前昵称相同
            /// </summary>
            NameNotTheSame = 160025,
            /// <summary>
            /// 昵称已存在
            /// </summary>
            NicknamesAlreadyExist = 160034,
            /// <summary>
            /// 玩家不存在
            /// </summary>
            PlayersNotExist = 160036,
            /// <summary>
            /// 昵称长度小于最小限制
            /// </summary>
            NameLengthLessMin = 160037,
            /// <summary>
            /// 昵称长度大于最大限制
            /// </summary>
            NameLengthGreaterMax = 160038,
            /// <summary>
            /// 含有特殊字符
            /// </summary>
            ContainsSpecialCharacters = 160039,
            /// <summary>
            /// 未查询到该玩家信息
            /// </summary>
            NotFoundAvatarId = 160050,

            #region 封禁 角色删除 161200 - 161299
            /// <summary>
            /// 玩家账号已删除，请联系客服处理
            /// </summary>
            PlayerIsRemove = 161201,
            /// <summary>
            /// 账号已被封禁，解封时间{0}
            /// </summary>
            NotForbidEndTime = 161202,
            #endregion

            #region GM
            /// <summary>
            /// 无GM权限
            /// </summary>
            CannotGMManagement = 169901,
            #endregion
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class AvatarBase
    {
        /// <summary>
        /// 
        /// </summary>
        public const string StreamSubscription = "Avatar.Generate";
    }
}
