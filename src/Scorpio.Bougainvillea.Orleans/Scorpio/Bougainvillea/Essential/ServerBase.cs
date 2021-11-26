using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Orleans.Streams;

using Sailina.Tang.Essential.Dtos;
using Sailina.Tang.Essential.StreamDatas;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Essential.Dtos.Servers;
using Scorpio.Bougainvillea.Tokens;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TServer"></typeparam>
    [ImplicitStreamSubscription(ServerBase.StreamSubscription)]
    public abstract class ServerBase<TServer> : GrainBase<TServer>, IServerBase, IIncomingGrainCallFilter
         where TServer : ServerBase<TServer>
    {
        private StreamSubscriptionHandle<LoginStatusNotify> _streamSubscriptionHandle;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="dateTimeProvider"></param>
        protected ServerBase(IServiceProvider serviceProvider, IDateTimeProvider dateTimeProvider) : base(serviceProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        private StreamSubscriptionHandle<ServerInfo> _handler;

        private ServerState _serverState;
        private readonly IDateTimeProvider _dateTimeProvider;

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(ServerBase.AvatarListStateName, ServerBase.AvatarListStateStorageName)]
        public IPersistentState<AvatarListState> AvatarList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(ServerBase.ServerInfoStateName, ServerBase.ServerInfoStateStorageName)]
        public IPersistentState<ServerInfo> ServerInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnActivateAsync()
        {
            _handler = await this.GetStreamAsync<ServerInfo>(this.GetPrimaryKey(), ServerBase.StreamSubscription)
                .SubscribeAsync(async (s, t) => await GenerateAsync(s));
            _streamSubscriptionHandle = await this.GetStreamAsync<LoginStatusNotify>(this.GetPrimaryKey(), AvatarBase.LoginResultStreamSubscription).SubscribeAsync((result, token) =>
            {
                var ava = AvatarList.State.FirstOrDefault(s => s.AvatarId == result.AvatarId);
                ava.Status = result.Status;
                return Task.CompletedTask;
            });
            _serverState = ServerState.GetServerState(this, ServerInfo.State.Status);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnDeactivateAsync()
        {
            await _handler?.UnsubscribeAsync();
            await _streamSubscriptionHandle?.UnsubscribeAsync();
            await base.OnDeactivateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual async Task GenerateAsync(ServerInfo info)
        {
            ServerInfo.State = info;
            await ServerInfo.WriteStateAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        public virtual async Task<int> GenerateAvatarAsync(GenerateInfo generateInfo)
        {

            var code = await CheckWords(generateInfo.Name);
            if (code != ErrorCode.None)
            {
                return (int)code;
            }
            if (AvatarList.State.Any(a => a.Name.Equals(generateInfo.Name)))
            {
                return (int)ErrorCode.NicknamesAlreadyExist;
            }
            var roleSettings = await GameSettingManager.GetAsync<RoleSetting>();
            if (!roleSettings.Any(r => r.Sex == generateInfo.Sex))
            {
                return (int)ErrorCode.GenerateAvatarLose;
            }
            var roleSetting = roleSettings.FirstOrDefault(r => r.Sex == generateInfo.Sex);
            if (!roleSetting.Heads.Contains(generateInfo.HeadId) || roleSetting.Image != generateInfo.Image)
            {
                return (int)ErrorCode.GenerateAvatarLoseHeadDoNotChoose;
            }
            var current = ServiceProvider.GetService<ICurrentUser>();
            await this.GetStreamAsync<GenerateInfo>(0, current.AvatarId, AvatarBase.GenerateStreamSubscription).OnNextAsync(generateInfo);
            return (int)ErrorCode.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public virtual Task<bool> IsGenerated(long avatarId)
        {
            return Task.FromResult(AvatarList.State.Any(a => a.AvatarId == avatarId));
        }

        private async Task<ErrorCode> CheckWords(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return ErrorCode.InputCharactersAreDisallowedToBeNull;
            }
            var check = ServiceProvider.GetService<IWordsCheck>();
            if (check.HasIllegalWords(text))
            {
                return ErrorCode.ContainsMaskingCharacters;
            }
            var length = check.GetWordsLength(text);
            var minMaxLength = await ServiceProvider.GetService<ISettingManager>()
                .GetAsync<List<int>>(AvatarSettingDefinitionConsts.NickNameMinMaxLength);
            if (length > minMaxLength[1])
            {
                return ErrorCode.NameLengthGreaterMax;
            }
            if (length < minMaxLength[0])
            {
                return ErrorCode.NameLengthLessMin;
            }
            return ErrorCode.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async ValueTask<ServerStatus> OpenAsync()
        {
            await _serverState.OpenAsync();
            return ServerInfo.State.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async ValueTask<ServerStatus> MaintenanceAsync()
        {
            await _serverState.MaintenanceAsync();
            return ServerInfo.State.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public ValueTask<AvatarInfo> GetAvatarAsync(int avatarId) => ValueTask.FromResult(AvatarList.State.FirstOrDefault(a => a.AvatarId == avatarId));


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async ValueTask<ServerStatus> CloseAsync()
        {
            await _serverState.CloseAsync();
            return ServerInfo.State.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ValueTask<DateTimeOffset> GetServerTimeAsync()
        {
            return ValueTask.FromResult(DateTimeOffset.Now.Add(ServerInfo.State.ServerTimeOffset));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverTimeOffset"></param>
        /// <returns></returns>
        public async ValueTask SetServerTimeOffset(TimeSpan serverTimeOffset)
        {
            ServerInfo.State.ServerTimeOffset = serverTimeOffset;
            await ServerInfo.WriteStateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask BeginInitializeAsync() => ValueTask.CompletedTask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async ValueTask<EnterResult> CheckUserAsync(UserData userData)
        {
            var user = AvatarList.State.FirstOrDefault(u => u.UserId == userData.UserId);
            if (user == null)
            {
                return new EnterResult
                {
                    AvatarId = 0,
                    CanRegister = ServerInfo.State.CanRegister,
                    Exists = false,
                    ServerId = (int)this.GetPrimaryKeyLong(),
                    ServerTime = DateTimeOffset.Now.Add(ServerInfo.State.ServerTimeOffset)
                };
            }
            else
            {
                return new EnterResult
                {
                    AvatarId = user.AvatarId,
                    CanRegister = ServerInfo.State.CanRegister,
                    CanLogin = user.ForbidExpired < await _dateTimeProvider.GetNowAsync(),
                    Exists = true,
                    ServerId = (int)this.GetPrimaryKeyLong(),
                    ServerTime = DateTimeOffset.Now.Add(ServerInfo.State.ServerTimeOffset)
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OneWay]
        public async ValueTask BeginLoginAsync(LoginData request)
        {
            var ava = AvatarList.State.FirstOrDefault(a => a.AvatarId == request.AvatarId);
            if (ava != null && ava.Status != AvatarInfoStatus.OnLoging)
            {
                ava.Status = AvatarInfoStatus.OnLoging;
                await this.GetStreamAsync<LoginData>(0, request.AvatarId, AvatarBase.GenerateStreamSubscription).OnNextAsync(request);
            }
        }

        async Task IIncomingGrainCallFilter.Invoke(IIncomingGrainCallContext context)
        {
            using (CurrentServer.Use((int)context.Grain.GetPrimaryKeyLong()))
            {
                await context.Invoke();
            }
        }

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


        private class ServerState
        {
            public ServerBase<TServer> Server { get; init; }
            public virtual ValueTask OpenAsync()
            {
                Server.ServerInfo.State.Status = ServerStatus.Normal;
                Server._serverState = new ServerNormalState { Server = Server };
                return ValueTask.CompletedTask;
            }

            public virtual ValueTask MaintenanceAsync()
            {
                Server.ServerInfo.State.Status = ServerStatus.Maintenance;
                Server._serverState = new ServerMaintenancedState { Server = Server };
                return ValueTask.CompletedTask;
            }
            public virtual ValueTask CloseAsync()
            {
                Server.ServerInfo.State.Status = ServerStatus.Closed;
                Server._serverState = new ServerClosedState { Server = Server };
                return ValueTask.CompletedTask;

            }

            public static ServerState GetServerState(ServerBase<TServer> server, ServerStatus status)
            {
                return status switch
                {
                    ServerStatus.Normal => new ServerNormalState { Server = server },
                    ServerStatus.AwaitOpen => new ServerAwaitOpenState { Server = server },
                    ServerStatus.Maintenance => new ServerMaintenancedState { Server = server },
                    ServerStatus.Closed => new ServerClosedState { Server = server },
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private class ServerNormalState : ServerState
        {
            public override ValueTask OpenAsync() => ValueTask.CompletedTask;
        }
        private class ServerClosedState : ServerState
        {
            public override ValueTask CloseAsync() => ValueTask.CompletedTask;
        }

        private class ServerMaintenancedState : ServerState
        {
            public override ValueTask MaintenanceAsync() => ValueTask.CompletedTask;
        }

        private class ServerAwaitOpenState : ServerState
        {
            public override ValueTask MaintenanceAsync() => ValueTask.CompletedTask;

            public override ValueTask CloseAsync() => ValueTask.CompletedTask;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AvatarListState : SortedSet<AvatarInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        public AvatarListState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public AvatarListState(IComparer<AvatarInfo> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public AvatarListState(IEnumerable<AvatarInfo> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public AvatarListState(IEnumerable<AvatarInfo> collection, IComparer<AvatarInfo> comparer) : base(collection, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AvatarListState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ServerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public const string StreamSubscription = "Server.Generate";

        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarListStateStorageName = "AvatarListStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarListStateName = "AvatarListState";

        /// <summary>
        /// 
        /// </summary>
        public const string ServerInfoStateStorageName = "ServerInfoStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string ServerInfoStateName = "ServerInfoState";

    }
}
