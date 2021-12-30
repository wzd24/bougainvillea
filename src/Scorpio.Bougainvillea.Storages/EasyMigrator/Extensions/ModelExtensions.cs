using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;


namespace EasyMigrator.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    static public class ModelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableType"></param>
        /// <returns></returns>
        static public Parsing.Context ParseTable(this Type tableType) => Parsing.Parser.Current.ParseTableType(tableType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> DefinedInPoco(this IEnumerable<Column> columns) => columns.Where(c => c.DefinedInPoco);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> ForeignKeys(this IEnumerable<Column> columns) => columns.Where(c => c.ForeignKey != null);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> MaxLength(this IEnumerable<Column> columns) => columns.Where(c => (c.Type == DbType.AnsiString || c.Type == DbType.String) && c.Length == int.MaxValue);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> WithPrecision(this IEnumerable<Column> columns) => columns.Where(c => c.Precision != null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> WithCustomAutoIncrement(this IEnumerable<Column> columns) => columns.Where(c => c.IsCustomAutoIncrement());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        static public IEnumerable<Column> WithoutCustomAutoIncrement(this IEnumerable<Column> columns) => columns.Where(c => !c.IsCustomAutoIncrement());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        static public bool IsDefaultAutoIncrement(this Column column) => column.AutoIncrement?.Seed == 1 && column.AutoIncrement?.Step == 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        static public bool IsCustomAutoIncrement(this Column column) => column.AutoIncrement != null && column.AutoIncrement.Seed != 1 && column.AutoIncrement.Step != 1;
    }
}
