using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public interface IIndex
    {

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        bool Clustered { get; }

        /// <summary>
        /// 
        /// </summary>
        bool Unique { get; }

        /// <summary>
        /// 
        /// </summary>
        string Where { get; }

        /// <summary>
        /// 
        /// </summary>
        string With { get; }

        /// <summary>
        /// 
        /// </summary>
        IIndexColumn[] Columns { get; }

        /// <summary>
        /// 
        /// </summary>
        IIndexColumn[] Includes { get; }
    }
}
