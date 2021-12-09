using System;
using System.Collections.Generic;
using System.Text;

namespace Sailina.Tang.Essential.Avatars
{
    /// <summary>
    /// 
    /// </summary>
    public enum ErrorCode
    {
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
        /// 地址已修改，无法变更
        /// </summary>
         UnableToChangeAddress = 160004,
        /// <summary>
        /// 地址修改，非法地址ID
        /// </summary>
         IllegalAddressId = 160005,
        /// <summary>
        /// 修改形象，形象暂未解锁
        /// </summary>
         TemporarilyNotUnlockImage = 160006,
        /// <summary>
        /// 修改头像框，头像框未解锁
        /// </summary>
         TemporarilyNotUnlockHeadFrame = 160007,
        /// <summary>
        /// 修改头像，头像未解锁
        /// </summary>
         TemporarilyNotUnlockHead = 160008,
        /// <summary>
        /// 更新称号，称号未解锁
        /// </summary>
         TemporarilyNotUnlockTitle = 160009,
        /// <summary>
        /// 升级道具不足，扣除失败
        /// </summary>
         UpgradePropsNotEnough = 160010,
        /// <summary>
        /// 货币不足
        /// </summary>
         CurrencyInsufficient = 160011,
        /// <summary>
        /// 货币类型不存在
        /// </summary>
         CurrencyTypesNotExist = 160012,
        /// <summary>
        /// 时装已达最高级，无法提升
        /// </summary>
         FashionLevelMax = 160013,
        /// <summary>
        /// 未解锁时装
        /// </summary>
         FashionNotExist = 160014,
        /// <summary>
        /// 包含铭感词
        /// </summary>
         ContainsMaskingCharacters = 160015,
        /// <summary>
        /// 创角失败
        /// </summary>
         GenerateAvatarLose = 160016,
        /// <summary>
        /// 未找到名士
        /// </summary>
         NotFoundHero = 160017,
        /// <summary>
        /// 形象已解锁
        /// </summary>
         UnlockedImage = 160018,
        /// <summary>
        /// 头像已解锁
        /// </summary>
         UnlockedHead = 160019,
        /// <summary>
        /// 头像框已解锁
        /// </summary>
         UnlockedHeadFrame = 160020,
        /// <summary>
        /// 时装已解锁
        /// </summary>
         UnlockedFashion = 160021,
        /// <summary>
        /// 已拥有该名士
        /// </summary>
         UnlockedHero = 160022,
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
        /// 商铺已存在
        /// </summary>
         UnlockedShop = 160026,
        /// <summary>
        /// 身份升级赚速不足
        /// </summary>
         UpgradeConditionNotMetEarnSpeed = 160027,
        /// <summary>
        /// 身份升级声望不足
        /// </summary>
         UpgradeConditionNotMetPrestige = 160028,
        /// <summary>
        /// 当前形象已穿戴，请选择其他形象
        /// </summary>
         ImageWornPleaseSelectEther = 160029,
        /// <summary>
        /// 当前头像已穿戴，请选择其他头像
        /// </summary>
         HeadWornPleaseSelectEther = 160030,
        /// <summary>
        /// 当前头像框已穿戴，请选择其他头像框
        /// </summary>
         HeadFrameWornPleaseSelectEther = 160031,
        /// <summary>
        /// 当前时装已穿戴，请选择其他时装
        /// </summary>
         FashionWornPleaseSelectEther = 160032,
        /// <summary>
        /// 当前称号已穿戴，请选择其他称号
        /// </summary>
         TitleWornPleaseSelectEther = 160033,
        /// <summary>
        /// 昵称已存在
        /// </summary>
         NicknamesAlreadyExist = 160034,
        /// <summary>
        /// 未找到玩家商铺
        /// </summary>
         NotFoundShop = 160035,
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
        /// 名士暂未解锁
        /// </summary>
        AreUnlockedHero = 160040,
        /// <summary>
        /// 商铺暂未解锁
        /// </summary>
        AreUnlockedShop = 160041,
         /// <summary>
         /// 兑换配置不存在
         /// </summary>
        NotFoundExchange = 160042,
        /// <summary>
        /// 已解锁名士或美女
        /// </summary>
        AlreadyHeroOrBeauty = 160043,
        /// <summary>
        /// 事件配置不存在
        /// </summary>
        EventSettingNotFound = 160044,
        /// <summary>
        /// 初始事件异常
        /// </summary>
        StateEventIdException = 160045,
        /// <summary>
        /// 无法处理此类型事件
        /// </summary>
        EventTypeException = 160046,
        /// <summary>
        /// 派遣名士处理事件异常
        /// </summary>
        EventHerosSendException = 160047,
        /// <summary>
        /// 派遣美女处理事件异常
        /// </summary>
        EventBeautysSendException = 160048,
        /// <summary>
        /// 新手引导ID参数异常
        /// </summary>
        GuideIdException = 160049,
        /// <summary>
        /// 未查询到该玩家信息
        /// </summary>
        NotFoundAvatarId = 160050,

        /// <summary>
        /// 区服禁止注册新玩家
        /// </summary>
        CannotRegister = 160051,


        #region 名士战斗 161000 - 161099
        /// <summary>
        /// 对战NPC为空
        /// </summary>
        NPCIsNull = 161001,
        /// <summary>
        /// 出战NPC配置异常
        /// </summary>
        FightNPCException = 161002,
        /// <summary>
        /// 上阵名士为空
        /// </summary>
        TeamHerosIsNull = 161003,
        /// <summary>
        /// 未拥有名士
        /// </summary>
        NotHaveHero = 161004,
        /// <summary>
        /// 出战名士重复
        /// </summary>
        FightHerosRepetition = 161005,
        #endregion

        #region 图鉴 161100 - 161199
        /// <summary>
        /// 未找到图鉴奖励
        /// </summary>
        NotFoundIllustratedProgressAward = 161101,
        /// <summary>
        /// 已领取过该奖励
        /// </summary>
        AlreadyReceivedIllustratedAward = 161102,
        /// <summary>
        /// 未满足领奖条件
        /// </summary>
        UnmetReceiveReward = 161103,
        #endregion

        #region 财神送财 161200 - 161299
        /// <summary>
        /// 财神送财文本为空
        /// </summary>
        SendMoneyContentIsNullOrEmpty = 161200,
        /// <summary>
        /// 财神送财文本长度不足
        /// </summary>
        SendMoneyContentLengthMin = 161201,
        /// <summary>
        /// 财神送财文本长度超出
        /// </summary>
        SendMoneyContentLengthMax = 161202,
        /// <summary>
        /// 今日已使用财神送财
        /// </summary>
        ToDayHaveBeenUsedSendMoney = 161203,
        /// <summary>
        /// 未拥有可使用财神送财称号
        /// </summary>
        NotUnlockTitle = 161204,
        /// <summary>
        /// 未找到财神送财信息
        /// </summary>
        NotFoundSendMoneyInfo = 161205,
        /// <summary>
        /// 财神送财信息已过期
        /// </summary>
        SendMoneyInfoIsOut = 161206,
        /// <summary>
        /// 已领取过财神送财奖励
        /// </summary>
        AlreadyReceiveSendMoneyReward = 161207,
        #endregion


        #region 创角七日奖励 161300 - 161399
        /// <summary>
        /// 未找到创角奖励
        /// </summary>
        NotFoundCreateReward = 161301,
        /// <summary>
        /// 已领取本日创角奖励
        /// </summary>
        AlreadyReceiveCreateReward = 161302,

        #endregion

        #region Token验证 161400 - 161499
        /// <summary>
        /// token过期
        /// </summary>
        TokenPastDue = 161401,
        /// <summary>
        /// token验证失败，在其他设备登录或登录已超时
        /// </summary>
        TokenVerifyFail = 161402,
        #endregion

        #region 登录
        /// <summary>
        /// 登录错误
        /// </summary>
        LoginError = 161501,
        #endregion

        #region 创角
        /// <summary>
        /// 创角异常
        /// </summary>
        GenerateError = 161601,
        #endregion

        #region 封禁 角色删除 161700 - 161799
        /// <summary>
        /// 玩家账号已删除，请联系客服处理
        /// </summary>
        PlayerIsRemove = 161701,
        /// <summary>
        /// 账号已被封禁，请联系客服处理
        /// </summary>
        NotForbidEndTime = 161702,
        #endregion

        #region GM
        /// <summary>
        /// 无GM权限
        /// </summary>
        CannotGMManagement = 169901,
        #endregion

    }
}
