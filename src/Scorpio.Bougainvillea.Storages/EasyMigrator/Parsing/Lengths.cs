using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing
{

    /// <summary>
    /// 
    /// </summary>
    public class Lengths
    {

        /// <summary>
        /// 
        /// </summary>
        public int Default { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Short { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Medium { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Long { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int this[Length length]
        {
            get {
                switch (length) {
                    case Length.Short: return Short;
                    case Length.Medium: return Medium;
                    case Length.Long: return Long;
                    case Length.Max: return Max;
                    default: return Default;
                }
            }
        }
    }
}
