using System.Collections;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PropsHandleContext
    {
        /// <summary>
        /// 
        /// </summary>
        public int PropId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Parameter { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
       
        /// <summary>
        /// 
        /// </summary>
        public string Reason { get; set; }
    }
}