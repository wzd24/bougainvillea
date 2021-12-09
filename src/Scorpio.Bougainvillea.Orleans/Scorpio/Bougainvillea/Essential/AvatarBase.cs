using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.CodeGeneration;
using Orleans.Runtime;
using Orleans.Storage;
using Orleans.Streams;
using Orleans.Streams.Core;

using Sailina.Tang.Essential.Dtos;
using Sailina.Tang.Essential.StreamDatas;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Setting;
using Scorpio.EventBus;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAvatar"></typeparam>
    /// <typeparam name="TAvatarState"></typeparam>
    /// <typeparam name="TEntityBase"></typeparam>
    [ImplicitStreamSubscription(AvatarBase.GenerateStreamSubscription)]
    [ImplicitStreamSubscription(AvatarBase.LoginStreamSubscription)]
    public abstract class AvatarBase<TAvatar, TAvatarState, TEntityBase> : GrainBase<TAvatar>, IAvatarBase, IIncomingGrainCallFilter
         where TAvatar : AvatarBase<TAvatar, TAvatarState, TEntityBase>
         where TAvatarState : AvatarStateBase<TEntityBase>, new()
        where TEntityBase : AvatarBaseEntityBase, new()
    {

        private StreamSubscriptionHandle<GenerateInfo> _generateHandler;
        private StreamSubscriptionHandle<LoginData> _loginHandler;

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(AvatarBase.AvatarStateName, AvatarBase.AvatarStateStorageName)]
        protected IPersistentState<TAvatarState> AvatarState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEventBus EventBus { get;}
        /// <summary>
        /// 
        /// </summary>
        public TAvatarState State => AvatarState.State;

        /// <summary>
        /// 
        /// </summary>
        protected AvatarOptions Options { get; }

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, ISubSystem> SubSystems { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        protected AvatarBase(IServiceProvider serviceProvider, IOptions<AvatarOptions> options)
            : base(serviceProvider)
        {
            SubSystems = new Dictionary<string, ISubSystem>();
            Options = options.Value;
            EventBus = serviceProvider.GetService<IEventBus>();
            InitSubSystems(serviceProvider);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifecycle"></param>
        public override void Participate(IGrainLifecycle lifecycle)
        {
            lifecycle.Subscribe<TAvatar>(GrainLifecycleStage.SetupState + 100, OnSubSystemSetup);
            base.Participate(lifecycle);
        }

        private async Task OnSubSystemSetup(CancellationToken arg)
        {
            await SubSystems.Values.ForEachAsync(async s =>
             {
                 await s.OnSetupAsync(this);
             });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        private void InitSubSystems(IServiceProvider serviceProvider)
        {
            Options.SubSystems.ForEach(s =>
            {
                SubSystems.AddOrUpdate(s.Name, _ =>
                {
                    var sub = ActivatorUtilities.CreateInstance(serviceProvider, s) as ISubSystem;

                    return sub;
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnActivateAsync()
        {
            _generateHandler = await this.GetStreamAsync<GenerateInfo>(this.GetPrimaryKey(), AvatarBase.GenerateStreamSubscription).SubscribeAsync(async (d, t) => await GenerateAsync(d));
            _loginHandler = await this.GetStreamAsync<LoginData>(this.GetPrimaryKey(), AvatarBase.LoginStreamSubscription).SubscribeAsync(async (d, t) =>
            {
                await LoginAsync(d);
            });
            await SubSystems.Values.ForEachAsync(async s =>
            {
                await s.InitializeAsync();
            });
            await base.OnActivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        protected virtual async ValueTask LoginAsync(LoginData d)
        {
            DailyReset();
            await LoginCoreAsync(d);
            await this.GetStreamAsync<LoginStatusNotify>(0, State.Base.ServerId, AvatarBase.LoginResultStreamSubscription).OnNextAsync(new LoginStatusNotify { AvatarId = d.AvatarId, Status = AvatarInfoStatus.OnLine });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        protected virtual ValueTask LoginCoreAsync(LoginData d)
        {
            return ValueTask.CompletedTask;
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual async void DailyReset()
        {
            if (await NeedDailyReset())
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract ValueTask<bool> NeedDailyReset();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnDeactivateAsync()
        {
            await _generateHandler?.UnsubscribeAsync();
            await _loginHandler?.UnsubscribeAsync();
            await base.OnDeactivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        public async virtual ValueTask<int> GenerateAsync(GenerateInfo generateInfo)
        {
            var roleSetting = (await GameSettingManager.GetAsync<RoleSetting>()).FirstOrDefault(r => r.Sex == generateInfo.Sex);
            if (roleSetting == null)
            {
                return (int)ErrorCode.ConfigurationDoesNotExist;
            }
            await InitAvatarBaseInfoAsync(generateInfo, roleSetting.HeadFrameId);
            await GenerateCoreAsync(generateInfo);
            await this.GetStreamAsync<LoginStatusNotify>(0, State.Base.ServerId, AvatarBase.LoginResultStreamSubscription).OnNextAsync(new LoginStatusNotify { AvatarId = this.GetPrimaryKeyLong(), Status = AvatarInfoStatus.OnLine });
            await PostGenerateAsync();
            return (int)ErrorCode.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        protected virtual ValueTask<int> GenerateCoreAsync(GenerateInfo generateInfo)
        {
            return ValueTask.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <param name="headFrameId"></param>
        /// <returns></returns>
        protected virtual async ValueTask InitAvatarBaseInfoAsync(GenerateInfo generateInfo, int headFrameId)
        {
            AvatarState.State.Base.Id = CurrentUser.AvatarId;
            AvatarState.State.Base.UserId = generateInfo.UserId;
            AvatarState.State.Base.ServerId = CurrentServer.ServerId;
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
        public virtual ValueTask PostGenerateAsync()
        {
            return ValueTask.CompletedTask;
        }

        async Task IIncomingGrainCallFilter.Invoke(IIncomingGrainCallContext context)
        {
            using (CurrentUser.Use(context.Grain.GetPrimaryKeyLong()))
            {
                await context.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ValueTask<IDictionary<string, object>> GetLoginInfoAsync();


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
        public const string GenerateStreamSubscription = "Avatar.Generate";

        /// <summary>
        /// 
        /// </summary>
        public const string LoginStreamSubscription = "Avatar.Login";

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarStateStorageName = "AvatarStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarStateName = "AvatarState";

        /// <summary>
        /// 
        /// </summary>
        public const string LoginResultStreamSubscription = "Avatar.Login.Result";
    }
}
