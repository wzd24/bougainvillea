using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EasyMigrator
{

    /// <summary>
    /// 
    /// </summary>
    public class IndexColumn : Parsing.Model.IIndexColumn
    {

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn(string columnName) : this(columnName, SortOrder.Unspecified) { }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn(string columnName, SortOrder direction)
        {
            ColumnName = columnName;
            Direction = direction;
        }


        /// <summary>
        /// 
        /// </summary>
        public string ColumnName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnNameWithDirection => GetColumnNameWithDirection(ColumnName, Direction);

        /// <summary>
        /// 
        /// </summary>
        public SortOrder Direction { get; }

        static private string GetColumnNameWithDirection(string columnName, SortOrder direction)
             => direction == SortOrder.Ascending ? $"{columnName} ASC" : direction == SortOrder.Descending ? $"{columnName} DESC" : columnName;
    }


    /// <summary>
    /// 
    /// </summary>
    public class Ascending : IndexColumn
    {

        /// <summary>
        /// 
        /// </summary>
        public Ascending(string columnName) : base(columnName, SortOrder.Ascending) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Descending : IndexColumn
    {

        /// <summary>
        /// 
        /// </summary>
        public Descending(string columnName) : base(columnName, SortOrder.Descending) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public class IndexColumn<TTable>
    {

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn(Expression<Func<TTable, object>> columnExpression) : this(columnExpression, SortOrder.Unspecified) { }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn(Expression<Func<TTable, object>> columnExpression, SortOrder direction)
        {
            ColumnExpression = columnExpression;
            Direction = direction;
        }


        /// <summary>
        /// 
        /// </summary>
        public Expression<Func<TTable, object>> ColumnExpression { get; }

        /// <summary>
        /// 
        /// </summary>
        public SortOrder Direction { get; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Ascending<TTable> : IndexColumn<TTable>
    {

        /// <summary>
        /// 
        /// </summary>
        public Ascending(Expression<Func<TTable, object>> columnExpression) : base(columnExpression, SortOrder.Ascending) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Descending<TTable> : IndexColumn<TTable>
    {

        /// <summary>
        /// 
        /// </summary>
        public Descending(Expression<Func<TTable, object>> columnExpression) : base(columnExpression, SortOrder.Descending) { }
    }
}
