using System;
using System.Collections.Generic;

using Dapper.Extensions;

using Scorpio.Bougainvillea.Data;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public class AvatarInfo : IEqualityComparer<AvatarInfo>, IComparable<AvatarInfo>, IModifiable
    {
        private bool _modified = false;
        private string _name;

        /// <summary>
        /// 
        /// </summary>
        [ExplicitKey]
        public long AvatarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => _name; set
            {
                _name = value;
                _modified = true;
            }
        }

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
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset ForbidExpired { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public AvatarInfoStatus Status { get; set; }

        bool IModifiable.Modified => _modified;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HashCode.Combine(AvatarId);
        int IComparable<AvatarInfo>.CompareTo(AvatarInfo other) => AvatarId.CompareTo(other.AvatarId);
        bool IEqualityComparer<AvatarInfo>.Equals(AvatarInfo x, AvatarInfo y) => x.AvatarId.Equals(y.AvatarId);
        int IEqualityComparer<AvatarInfo>.GetHashCode(AvatarInfo obj) => obj.GetHashCode();
        void IModifiable.ResetModifyState() => _modified = false;
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
        OnLoging,

        /// <summary>
        /// 
        /// </summary>
        OnLine
    }
}
