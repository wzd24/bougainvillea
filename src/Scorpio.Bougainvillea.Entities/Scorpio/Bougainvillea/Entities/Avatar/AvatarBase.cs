using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Scorpio.Bougainvillea.Entities.Avatar
{

    /// <summary>
    /// 角色基础信息
    /// </summary>
    public class AvatarBase
    {

        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 玩家Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色所属区服Id
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 玩家GUID，用于关联用户中心账号的标识
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 角色昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 当前穿戴头像ID
        /// </summary>
        public int HeadId { get; set; }

        /// <summary>
        /// 当前穿戴头像框ID
        /// </summary>
        public int HeadFrameId { get; set; }

        /// <summary>
        /// 当前穿戴称号ID
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// 当前穿戴时装ID
        /// </summary>
        public int FashionId { get; set; }

        /// <summary>
        /// 地址ID 默认配置地址取杂项配置1100000
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// 角色VIP等级
        /// </summary>
        public int VipLevel { get; set; }

        /// <summary>
        /// 角色创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

    }
}
