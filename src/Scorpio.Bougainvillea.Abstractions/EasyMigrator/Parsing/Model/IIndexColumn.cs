using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public interface IIndexColumn
    {

        /// <summary>
        /// 
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// 
        /// </summary>
        string ColumnNameWithDirection { get; }

        /// <summary>
        /// 
        /// </summary>
        SortOrder Direction { get; }
    }
}
