using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public interface IForeignKey
    {

        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Table { get; }

        /// <summary>
        /// 
        /// </summary>
        string Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Rule? OnDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Rule? OnUpdate { get; set; }
    }
}
