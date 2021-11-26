using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

namespace Scorpio.Bougainvillea.Essential
{

    /// <summary>
    /// 角色基础信息
    /// </summary>
    public abstract class AvatarBaseEntityBase
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [ExplicitKey]
        public long Id { get; set; }

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
        public string AccountId { get; set; }

        /// <summary>
        /// 角色昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色性别
        /// </summary>
        public Sex Sex { get; set; }

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
        /// 玩家形象
        /// </summary>
        public int Image { get; set; }


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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityBase"></typeparam>
    public abstract class AvatarStateBase<TEntityBase> where TEntityBase : AvatarBaseEntityBase,new()
    {
        /// <summary>
        /// 
        /// </summary>
        public TEntityBase Base { get; set; }=new TEntityBase();
    }
}
