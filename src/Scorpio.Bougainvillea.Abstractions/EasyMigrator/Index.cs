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
    public class Index : Parsing.Model.IIndex
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Clustered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string With { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn[] Columns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn[] Includes { get; set; }
        Parsing.Model.IIndexColumn[] Parsing.Model.IIndex.Columns => Columns;
        Parsing.Model.IIndexColumn[] Parsing.Model.IIndex.Includes => Includes;


        /// <summary>
        /// 
        /// </summary>
        public Index() { }

        /// <summary>
        /// 
        /// </summary>
        public Index(params string[] columnNamesAndDirection) 
            : this(ConvertToColumns(columnNamesAndDirection).ToArray(), null) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(string[] columnNamesAndDirection, string[] includes) 
            : this(ConvertToColumns(columnNamesAndDirection).ToArray(), ConvertToColumns(includes).ToArray()) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(params IndexColumn[] columns) 
            : this(columns, null) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(IndexColumn[] columns, IndexColumn[] includes)
        {
            Columns = columns;
            Includes = includes ?? new IndexColumn[0];
        }


        /// <summary>
        /// 
        /// </summary>
        static protected IEnumerable<IndexColumn> ConvertToColumns(IEnumerable<string> columnNamesWithDirection)
        {
            foreach (var c in columnNamesWithDirection ?? Enumerable.Empty<string>()) {
                if (c.EndsWith(" ASC"))
                    yield return new IndexColumn(c.Substring(0, c.Length - " ASC".Length), SortOrder.Ascending);
                else if (c.EndsWith(" DESC"))
                    yield return new IndexColumn(c.Substring(0, c.Length - " DESC".Length), SortOrder.Descending);
                else
                    yield return new IndexColumn(c);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class Index<TTable>
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Clustered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string With { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn<TTable>[] Columns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IndexColumn<TTable>[] Includes { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Index() { }

        /// <summary>
        /// 
        /// </summary>
        public Index(params Expression<Func<TTable, object>>[] columns) 
            : this(columns.Select(c => new IndexColumn<TTable>(c)).ToArray()) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(Expression<Func<TTable, object>>[] columns, Expression<Func<TTable, object>>[] includes) 
            : this(columns.Select(c => new IndexColumn<TTable>(c)).ToArray(), includes.Select(c => new IndexColumn<TTable>(c)).ToArray()) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(params IndexColumn<TTable>[] columns) 
            : this(columns, null) { }

        /// <summary>
        /// 
        /// </summary>
        public Index(IndexColumn<TTable>[] columns, IndexColumn<TTable>[] includes)
        {
            Columns = columns;
            Includes = includes ?? new IndexColumn<TTable>[0];
        }
    }
}
