using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans.Runtime;
using Orleans.Storage;

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
    public abstract class AvatarBase<TAvatar, TAvatarState> : GrainBase<TAvatar>, IAvatarBase
        where TAvatar : AvatarBase<TAvatar, TAvatarState>
        where TAvatarState : AvatarEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AvatarBaseStateStorage = "AvatarBaseStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string AvatarBaseState = "AvatarBaseState";

        /// <summary>
        /// 
        /// </summary>
        protected ICurrentUser CurrentUser { get; }
        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(AvatarBaseState, AvatarBaseStateStorage)]
        protected IPersistentState<TAvatarState> AvatarState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected AvatarBase()
        {
            CurrentUser = ServiceProvider.GetService<ICurrentUser>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        public async virtual Task<int> Generate(GenerateInfo generateInfo)
        {
            var code = await CheckWords(generateInfo.Name);
            if (code != ErrorCode.None)
            {
                return (int)code;
            }
            await InitAvatarBaseInfo(generateInfo, 0);
            var result = await GenerateCore(generateInfo);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        protected virtual Task<int> GenerateCore(GenerateInfo generateInfo)
        {
            return Task.FromResult(0);
        }

        private async Task InitAvatarBaseInfo(GenerateInfo generateInfo, int headFrameId)
        {
            AvatarState.State.Id = CurrentUser.AvatarId;
            AvatarState.State.UserId = CurrentUser.UserId;
            AvatarState.State.ServerId = CurrentUser.ServerId;
            AvatarState.State.NickName = generateInfo.Name;
            AvatarState.State.Image = generateInfo.Image;
            AvatarState.State.Level = 1;
            AvatarState.State.CreateDate = DateTime.Now;
            AvatarState.State.Sex = generateInfo.Sex;
            AvatarState.State.HeadId = generateInfo.HeadId;
            AvatarState.State.HeadFrameId = headFrameId;
            await AvatarState.WriteStateAsync();
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
            var minMax = (await GameSettingManager.GetAsync<GameSetting>())[1100001].Value;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsGenerated()
        {
            return Task.FromResult(AvatarState.RecordExists);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task PostGenerate()
        {
            throw new NotImplementedException();
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
}
