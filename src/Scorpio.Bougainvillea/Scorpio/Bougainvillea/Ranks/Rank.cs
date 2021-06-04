using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Ranks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Rank : IRank
    {
        private readonly ISkipList<RankItem> _list;

        /// <summary>
        /// 
        /// </summary>
        protected abstract Func<RankItem> Min { get; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract Func<RankItem> Max { get; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract int MaxLength { get; }
        protected Rank()
        {
            _list = new SkipList<RankItem>(Min, Max, MaxLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual (int rank, RankItem data) GetRank(RankItem item)
        {
            return (_list.IndexOf(item), item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<RankItem> GetRanks(int count)
        {
            return _list.GetSegment(0, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void SetRank(RankItem item)
        {
            _list.Set(item);
        }
    }
}
