using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public interface IPrecision
    {

        /// <summary>
        /// 
        /// </summary>
        int Precision { get; }

        /// <summary>
        /// 
        /// </summary>
        int Scale { get; }
    }
}
