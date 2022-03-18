using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Parsing
{

    /// <summary>
    /// 
    /// </summary>
    public partial class Conventions
    {

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, string> TableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, PropertyInfo, string> ColumnName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, IEnumerable<Column>> PrimaryKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, string> PrimaryKeyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, string> PrimaryKeyNameByTableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, string> PrimaryKeyColumnName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, Column, string> ForeignKeyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, string, string> ForeignKeyNameByTableAndColumnNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, IEnumerable<Column>, string> IndexNameByColumns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, IEnumerable<string>, string> IndexNameByTableAndColumnNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, ITypeMap> TypeMap { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, Column, Lengths> ColumnLengths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, Column, Lengths> PrecisionLengths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, Column, Lengths> ScaleLengths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, Column, IPrecision> DefaultPrecision { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<Context, bool> IndexForeignKeys { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Conventions Clone()
            => new Conventions {
                TableName = TableName,
                ColumnName = ColumnName,
                PrimaryKey = PrimaryKey,
                PrimaryKeyName = PrimaryKeyName,
                PrimaryKeyNameByTableName = PrimaryKeyNameByTableName,
                PrimaryKeyColumnName = PrimaryKeyColumnName,
                ForeignKeyName = ForeignKeyName,
                ForeignKeyNameByTableAndColumnNames = ForeignKeyNameByTableAndColumnNames,
                IndexNameByColumns = IndexNameByColumns,
                IndexNameByTableAndColumnNames = IndexNameByTableAndColumnNames,
                TypeMap = TypeMap,
                ColumnLengths = ColumnLengths,
                PrecisionLengths = PrecisionLengths,
                ScaleLengths = ScaleLengths,
                DefaultPrecision = DefaultPrecision,
                IndexForeignKeys = IndexForeignKeys,
            };
    }
}
