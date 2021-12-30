using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Extensions;


namespace EasyMigrator.Parsing.Model
{

    /// <summary>
    /// 
    /// </summary>
    public class Table
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasPrimaryKey => Columns.PrimaryKey().Any();

        /// <summary>
        /// 
        /// </summary>
        public string PrimaryKeyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PrimaryKeyIsClustered { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public IList<Column> Columns { get; set; } = new List<Column>();

        /// <summary>
        /// 
        /// </summary>
        public IList<IIndex> Indices { get; set; } = new List<IIndex>();
    }
}
