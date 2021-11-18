using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TServer"></typeparam>
    [ImplicitStreamSubscription(ServerBase.StreamSubscription)]
    public abstract class ServerBase<TServer> : GrainBase<TServer>, IServerBase
         where TServer : ServerBase<TServer>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected ServerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

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

        /// <summary>
        /// 
        /// </summary>
        public const string ServerInfoStateStorageName = "ServerInfoStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string ServerInfoStateName = "ServerInfoState";


        private StreamSubscriptionHandle<ServerInfo> _handler;

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(AvatarListStateName, AvatarListStateStorageName)]
        public IPersistentState<AvatarListState> AvatarList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(ServerInfoStateName, ServerInfoStateStorageName)]
        public IPersistentState<ServerInfo> ServerInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnActivateAsync()
        {
            _handler = await this.GetStreamAsync<ServerInfo>(this.GetPrimaryKey(), ServerBase.StreamSubscription)
                .SubscribeAsync(async (s, t) => await GenerateAsync(s));
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
        public virtual async Task<int> GenerateAvatar(GenerateInfo generateInfo)
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
            await this.GetStreamAsync<GenerateInfo>(0, current.AvatarId, AvatarBase.StreamSubscription).OnNextAsync(generateInfo);
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

        /// <summary>
        /// 
        /// </summary>
        public class AvatarListState : SortedSet<AvatarInfo>
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public class AvatarInfo : IEqualityComparer<AvatarInfo>, IComparable<AvatarInfo>
        {
            /// <summary>
            /// 
            /// </summary>
            public long AvatarId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public long UserId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string AccountId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode() => HashCode.Combine(AvatarId);
            int IComparable<AvatarInfo>.CompareTo(AvatarInfo other) => AvatarId.CompareTo(other.AvatarId);
            bool IEqualityComparer<AvatarInfo>.Equals(AvatarInfo x, AvatarInfo y) => x.AvatarId.Equals(y.AvatarId);
            int IEqualityComparer<AvatarInfo>.GetHashCode(AvatarInfo obj) => obj.GetHashCode();
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

    }
}
