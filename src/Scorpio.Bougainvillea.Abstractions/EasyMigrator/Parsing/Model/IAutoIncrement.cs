using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public interface IAutoIncrement
    {

        /// <summary>
        /// 
        /// </summary>
        long Seed { get; }

        /// <summary>
        /// 
        /// </summary>
        long Step { get; }
    }
}
