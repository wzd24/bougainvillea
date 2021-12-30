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
    public class Column
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CustomType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSparse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IAutoIncrement AutoIncrement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPrecision Precision { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IForeignKey ForeignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal bool DefinedInPoco { get; set; }
    }
}
