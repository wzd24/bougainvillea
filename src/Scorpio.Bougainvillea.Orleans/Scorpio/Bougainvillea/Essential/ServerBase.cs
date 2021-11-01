using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Runtime;

using Scorpio.Bougainvillea.Essential.Dtos;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TServer"></typeparam>
    public abstract class ServerBase<TServer>:GrainBase<TServer>, IServerBase
        where TServer:ServerBase<TServer>
    {
        /// <summary>
        /// 
        /// </summary>

        [PropertyPersistentState("AvatarListState", "AvatarListStateStorage")]
        public IPersistentState<AvatarListState> AvatarList { get; set; }

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
            if (AvatarList.State.Any(a=>a.Name.Equals(generateInfo.Name)))
            {
                return (int)ErrorCode.NicknamesAlreadyExist;
            }
            return 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public async virtual Task<bool> IsGenerated(long avatarId)
        {
            return false;
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
            if (length > 20)
            {
                return ErrorCode.NameLengthGreaterMax;
            }
            if (length < 4)
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
        public class AvatarListState:SortedSet<AvatarInfo>
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public class AvatarInfo: IEqualityComparer<AvatarInfo>,IComparable<AvatarInfo>
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
}
