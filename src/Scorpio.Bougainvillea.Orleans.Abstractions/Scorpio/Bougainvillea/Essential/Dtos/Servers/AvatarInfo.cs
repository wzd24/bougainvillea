using System;
using System.Collections.Generic;

using Dapper.Extensions;

using Scorpio.Bougainvillea.Data;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public class AvatarInfo : IEqualityComparer<AvatarInfo>, IComparable<AvatarInfo>
    {

        /// <summary>
        /// 
        /// </summary>
        [ExplicitKey]
        public virtual long AvatarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTimeOffset ForbidExpired { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public virtual AvatarInfoStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public virtual string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(AvatarId);
        int IComparable<AvatarInfo>.CompareTo(AvatarInfo other) => AvatarId.CompareTo(other.AvatarId);
        bool IEqualityComparer<AvatarInfo>.Equals(AvatarInfo x, AvatarInfo y) => x.AvatarId.Equals(y.AvatarId);
        int IEqualityComparer<AvatarInfo>.GetHashCode(AvatarInfo obj) => obj.GetHashCode();
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AvatarInfoStatus
    {
        /// <summary>
        /// 
        /// </summary>
        OffLine,
        /// <summary>
        /// 
        /// </summary>
        OnRegistering,

        /// <summary>
        /// 
        /// </summary>
        RegisterFaild,

        /// <summary>
        /// 
        /// </summary>
        OnLoging,
        /// <summary>
        /// 
        /// </summary>
        LoginFaild,


        /// <summary>
        /// 
        /// </summary>
        OnLine
    }
}
