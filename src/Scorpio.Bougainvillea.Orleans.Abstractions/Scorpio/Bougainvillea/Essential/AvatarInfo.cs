using System;
using System.Collections.Generic;

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
