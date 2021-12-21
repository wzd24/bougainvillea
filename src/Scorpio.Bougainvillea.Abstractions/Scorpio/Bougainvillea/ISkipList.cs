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
    /// <typeparam name="T"></typeparam>
    public interface ISkipList<T> : IEnumerable<T>
        where T:IComparable<T>,IEquatable<T>
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
        /// <param name="item"></param>
        /// <returns></returns>
        int IndexOf(T  item);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Set(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<T> GetSegment(int begin, int count);
    }
}
