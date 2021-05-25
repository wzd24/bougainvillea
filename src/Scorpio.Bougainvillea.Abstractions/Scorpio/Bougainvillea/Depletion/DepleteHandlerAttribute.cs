using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Depletion
{
    /// <summary>
    /// 
    /// </summary>
   [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public sealed class DepleteHandlerAttribute : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depletion"></param>
        public DepleteHandlerAttribute(params int[] depletion)
        {
            Depletion = depletion;
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Depletion { get; }
    }
}
