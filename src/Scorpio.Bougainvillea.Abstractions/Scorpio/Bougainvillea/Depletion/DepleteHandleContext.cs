using System.Collections;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Depletion
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DepleteHandleContext
    {
        /// <summary>
        /// 
        /// </summary>
        public ICollection<int> Depletion { get; set; }

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
        public IList AdditionDatas { get; } = new List<object>();
    }
}