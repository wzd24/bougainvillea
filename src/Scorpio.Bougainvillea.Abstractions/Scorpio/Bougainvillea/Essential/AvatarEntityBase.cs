using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using EasyMigrator;

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
        [Pk, NotNull]
        public virtual long Id { get; set; }

        /// <summary>
        /// 玩家Id
        /// </summary>
        [NotNull]
        public virtual int UserId { get; set; }

        /// <summary>
        /// 角色所属区服Id
        /// </summary>
        [NotNull]
        public virtual int ServerId { get; set; }

        /// <summary>
        /// 玩家GUID，用于关联用户中心账号的标识
        /// </summary>
        [Length(100)]
        public virtual string AccountId { get; set; }

        /// <summary>
        /// 角色昵称
        /// </summary>
        [Length(100)]
        public virtual string NickName { get; set; }

        /// <summary>
        /// 角色性别
        /// </summary>
        [DbType(System.Data.DbType.Int16)]
        public virtual Sex Sex { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public virtual int Level { get; set; }


        /// <summary>
        /// 当前穿戴头像ID
        /// </summary>
        public virtual int HeadId { get; set; }

        /// <summary>
        /// 当前穿戴头像框ID
        /// </summary>
        public virtual int HeadFrameId { get; set; }

        /// <summary>
        /// 玩家形象
        /// </summary>
        public virtual int Image { get; set; }


        /// <summary>
        /// 角色VIP等级
        /// </summary>
        public virtual int VipLevel { get; set; }

        /// <summary>
        /// 角色创建时间
        /// </summary>
       [DbType(System.Data.DbType.Int64)]
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [DbType(System.Data.DbType.Int64)]
        public virtual DateTime UpdateTime { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityBase"></typeparam>
    public abstract class AvatarStateBase<TEntityBase> where TEntityBase : AvatarBaseEntityBase, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public TEntityBase Base { get; set; } = new TEntityBase();
    }
}
