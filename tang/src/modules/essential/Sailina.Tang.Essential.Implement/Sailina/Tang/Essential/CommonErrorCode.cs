using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public enum CommonErrorCode
    {
        #region 基础错误码
        /// <summary>
        /// 功能未解锁
        /// </summary>
        NotUnlock = 100001,
        /// <summary>
        /// 参数错误（接口接收到非预期的参数）
        /// </summary>
        ParaError = 100002,
        /// <summary>
        /// 已达限购数量
        /// </summary>
        BuyLimit = 100003,
        /// <summary>
        /// 未实现
        /// </summary>
        FeatureNotImplement = 100004,
        /// <summary>
        /// 包含屏蔽词
        /// </summary>
        ContainsBadWord = 100005,
        /// <summary>
        /// 包含特殊字符
        /// </summary>
        ContainSpecialSymbol = 100006,
        /// <summary>
        /// 未过冷却时间
        /// </summary>
        InCoolTime = 100007,
        /// <summary>
        /// 字符串长度太小
        /// </summary>
        LengthLessMin = 100008,
        /// <summary>
        /// 字符串长度太大
        /// </summary>
        LengthGreaterMax = 100009,
        /// <summary>
        /// 未拥有GM权限
        /// </summary>
        UnauthorizedGM = 100010,
        /// <summary>
        /// 系统功能暂未解锁
        /// </summary>
        SystemUnauhorized = 100011,

        /// <summary>
        /// 数量或次数不足
        /// </summary>
        NumOrCountNotEnough = 100201,
        /// <summary>
        /// 已达上限
        /// </summary>
        AlreadyMax = 100202,
        #endregion

        #region 配置错误码
        /// <summary>
        /// 配置错误
        /// </summary>
        SettingError = 101001,
        /// <summary>
        /// 系统杂项配置表错误
        /// </summary>
        GameConfigSettingError = 101002,
        /// <summary>
        /// 系统解锁配置表错误
        /// </summary>
        SystemUnlockSettingError = 101003,
        #endregion

        #region 活动错误码
        /// <summary>
        /// 未找到活动配置
        /// </summary>
        Activity_NotFound = 102001,
        /// <summary>
        /// 活动已过期
        /// </summary>
        Activity_NotOnShow = 102002,
        /// <summary>
        /// 不在活动举行期间
        /// </summary>
        Activity_NotOnGoing = 102003,
        /// <summary>
        /// 未找到活动数据
        /// </summary>
        Activity_NotFoundData = 102004,
        /// <summary>
        /// 不在活动可操作时间段
        /// </summary>
        Activity_CanNotOperate = 102005,
        /// <summary>
        /// 活动未开启
        /// </summary>
        Activity_NotStarted = 102006,
        /// <summary>
        /// 活动已结束
        /// </summary>
        Activity_Ended = 102007,
        /// <summary>
        /// 未达到进入下一轮的条件
        /// </summary>
        Activity_CanNotNextRound = 102008,
        /// <summary>
        /// 奖励派发中前稍后
        /// </summary>
        Activity_RewardInStatistics = 102009,

        #region 活动任务 102050 - 102099
        /// <summary>
        /// 未达到任务领奖条件
        /// </summary>
        Activity_Task_Unfinished = 102051,
        /// <summary>
        /// 活动任务奖励已领取
        /// </summary>
        Activity_Task_AlreadyReward = 102052,
        /// <summary>
        /// 未找到活动任务配置
        /// </summary>
        Activity_Task_NotFound = 102053,
        #endregion

        #endregion

        #region 道具错误码
        /// <summary>
        /// 道具不足
        /// </summary>
        Prop_NotEnough = 104001,
        /// <summary>
        /// 道具不存在
        /// </summary>
        Prop_NotExist = 104002,
        /// <summary>
        /// 道具使用出错
        /// </summary>
        Prop_UseError = 104003,
        /// <summary>
        /// 道具不可使用
        /// </summary>
        Prop_NotCanUse = 104004,
        /// <summary>
        /// 道具合成出错
        /// </summary>
        Prop_SyntheticError = 104101,
        #endregion

        #region 奖励错误码
        /// <summary>
        /// 奖励已领取
        /// </summary>
        Reward_Received = 105001,
        /// <summary>
        /// 奖励领取错误
        /// </summary>
        Reward_Error = 105002,
        /// <summary>
        /// 无领取权限
        /// </summary>
        Reward_NoPermission = 105003,
        /// <summary>
        /// 应邀后才可领取奖励
        /// </summary>
        Reward_NoSeatError = 105004,
        #endregion

        #region 服务器错误码 106000 - 106999
        /// <summary>
        /// 服务器暂未开启
        /// </summary>
        ServerNotOpen = 106001,
        /// <summary>
        /// 服务器已关闭
        /// </summary>
        ServerIsClose = 106002,
        /// <summary>
        /// 服务器维护中
        /// </summary>
        ServerMaintenance = 106003,
        #endregion

        #region 商会错误码
        /// <summary>
        /// 商会创建失败
        /// </summary>
        Guild_GuildCreateFail = 201000,
        /// <summary>
        /// 已经加入其它商会
        /// </summary>
        Guild_JoinedGuild = 201001,
        /// <summary>
        /// 商会不存在
        /// </summary>
        Guild_NotExist = 201002,
        /// <summary>
        /// 商会已解散
        /// </summary>
        Guild_IsDissolved = 201003,
        /// <summary>
        /// 商会不允许自动加入
        /// </summary>
        Guild_NotAutoJoin = 201004,
        /// <summary>
        /// 商会已满员
        /// </summary>
        Guild_Full = 201005,
        /// <summary>
        /// 没有商会可自动加入
        /// </summary>
        Guild_NotGuildAutoJoin = 201006,
        /// <summary>
        /// 未找到指定成员信息
        /// </summary>
        Guild_NotFindMember = 201007,
        /// <summary>
        /// 职位权限不够进行此操作
        /// </summary>
        Guild_NotLeader = 201008,
        /// <summary>
        /// 未找到指定请求者信息
        /// </summary>
        Guild_NotFindRequester = 201009,
        /// <summary>
        /// 已有副会长
        /// </summary>
        Guild_HasVicePresident = 201010,
        /// <summary>
        /// 精英人数已达上限
        /// </summary>
        Guild_EliteFull = 201011,
        /// <summary>
        /// 职位没有变
        /// </summary>
        Guild_PositionNotChange = 201012,
        /// <summary>
        /// 不能将会长改为其它职位
        /// </summary>
        Guild_PresidentNoEmpty = 201013,
        /// <summary>
        /// 经营人数已达上限
        /// </summary>
        Guild_SidelineNumFull = 201014,
        /// <summary>
        /// 砍价商人已经结束
        /// </summary>
        Guild_BargainOver = 201015,
        /// <summary>
        /// 砍价人数已达上限
        /// </summary>
        Guild_BargainDoFull = 201016,
        /// <summary>
        /// 已砍价
        /// </summary>
        Guild_BargainDone = 201017,
        /// <summary>
        /// 未砍价不能购买
        /// </summary>
        Guild_BargainNotBuy = 201018,
        /// <summary>
        /// 已经购买不能重复购买
        /// </summary>
        Guild_BargainHasBuy = 201019,
        /// <summary>
        /// 今日建设进度已达上限
        /// </summary>
        Guild_MaxBuildProgress = 201020,
        /// <summary>
        /// 建设次数已达上限
        /// </summary>
        Guild_MaxBuildTimes = 201021,
        /// <summary>
        /// 名士已在议事厅
        /// </summary>
        Guild_HeroInHouse = 201022,
        /// <summary>
        /// 议事厅同职业派遣数已达上限
        /// </summary>
        Guild_PosionInHouseFull = 201023,
        /// <summary>
        /// 商会等级不足
        /// </summary>
        Guild_LvLow = 201024,
        /// <summary>
        /// 已达商贸路线开启次数上限
        /// </summary>
        Guild_TradeOpenMax = 201025,
        /// <summary>
        /// 商贸路线已开启
        /// </summary>
        Guild_TradeOpened = 201026,
        /// <summary>
        /// 商贸路线未开启
        /// </summary>
        Guild_TradeNotOpen = 201027,
        /// <summary>
        /// 商贸名士派遣次数已达上限
        /// </summary>
        Guild_TradeHeroSendTimesMax = 201028,
        /// <summary>
        /// 商贸已结束
        /// </summary>
        Guild_TradeEnd = 201029,
        /// <summary>
        /// 商贸队伍已返航
        /// </summary>
        Guild_TradeMiddle = 201030,
        /// <summary>
        /// 未找到会长选候选人
        /// </summary>
        Guild_NotFindNewPresident = 201031,
        /// <summary>
        /// 未加入商会
        /// </summary>
        Guild_NotInGuild = 201032,
        /// <summary>
        /// 商会允许自动加入无须申请
        /// </summary>
        Guild_CanAutoJoinNotRequest = 201033,
        /// <summary>
        /// 贡献未达到指定值，不可转让为会长
        /// </summary>
        Guild_CtrbNotEnoughToPresident = 201034,
        /// <summary>
        /// 贡献未达到指定值，不可当选副会长
        /// </summary>
        Guild_CtrbNotEnoughToVicePresident = 201035,
        /// <summary>
        /// 派遣名士中存在未解锁的名士，无法派遣
        /// </summary>
        Guild_HeroNotUnlocked = 201036,
        #endregion

        #region 公共服装
        /// <summary>
        /// 服装升级或解锁失败
        /// </summary>
        Skin_UnlockOrUpgradeFail = 107001,
        /// <summary>
        /// 服装穿戴失败
        /// </summary>
        Skin_WearFail = 107002,
        /// <summary>
        /// 重复穿戴
        /// </summary>
        Skin_RepeatWear = 107003,
        #endregion

        #region SDK
        /// <summary>
        /// 密匙验证失败
        /// </summary>
        SignError = 990001,
        #endregion

    }
}
