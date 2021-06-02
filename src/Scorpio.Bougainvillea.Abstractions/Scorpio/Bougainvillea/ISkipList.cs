using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TScore"></typeparam>
    public interface ISkipList<TKey,TScore> : IEnumerable<TKey>
        where TScore:IComparable<TScore>
    {
        /// <summary>
        /// 
        /// </summary>
        public int MaxLength { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel { get; }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        int IndexOf(TKey  key);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        int Set(TKey key,TScore score);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<TKey> GetSegment(int begin, int count);
    }
}
