using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Ranks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RankItem : IEquatable<RankItem>, IComparable<RankItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract int CompareTo(RankItem other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is null)
            {
                return false;
            }
            if (obj is RankItem rankItem)
            {
                return Equals(rankItem);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(ServerId, AvatarId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(RankItem other)
        {
            if (other is null)
            {
                return false;
            }
            return ServerId == other.ServerId && AvatarId == other.AvatarId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RankItem left, RankItem right)
        {
            if (left is null || right is null)
            {
                return false;
            }
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RankItem left, RankItem right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(RankItem left, RankItem right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(RankItem left, RankItem right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(RankItem left, RankItem right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(RankItem left, RankItem right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TScore"></typeparam>
    public abstract class RankItem<TScore> : RankItem
        where TScore : IComparable<TScore>
    {

        /// <summary>
        /// 
        /// </summary>
        public TScore Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareTo(RankItem other)
        {
            return Score.CompareTo((other as RankItem<TScore>).Score);
        }

    }
}
