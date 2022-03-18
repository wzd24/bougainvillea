﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMigrator.Extensions;
using EasyMigrator.Parsing.Model;
using FluentMigrator.Builders.Delete;


namespace EasyMigrator
{
    /// <summary>
    /// 
    /// </summary>
    static public class DeleteExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Delete"></param>
        static public void Table<T>(this IDeleteExpressionRoot Delete) => Delete.Table(typeof(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Delete"></param>
        /// <param name="tableType"></param>
        static public void Table(this IDeleteExpressionRoot Delete, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            Delete.ForeignKeys(table);
            Delete.Indexes(table);
            Delete.Table(table.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Delete"></param>
        static public void Columns<T>(this IDeleteExpressionRoot Delete) => Delete.Columns(typeof(T));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Delete"></param>
        /// <param name="tableType"></param>
        static public void Columns(this IDeleteExpressionRoot Delete, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            Delete.ForeignKeys(table);
            Delete.Indexes(table);
            Delete.Columns(table);
        }

        static private void Columns(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var c in table.Columns.DefinedInPoco())
                Delete.Column(c.Name).FromTable(table.Name);
        }

        static private void ForeignKeys(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var c in table.Columns.DefinedInPoco()) {
                var f = c.ForeignKey;
                if (f == null)
                    continue;

                if (f.Name != null)
                    Delete.ForeignKey(f.Name).OnTable(table.Name);
                else
                    Delete.ForeignKey()
                          .FromTable(table.Name)
                          .ForeignColumn(c.Name)
                          .ToTable(f.Table)
                          .PrimaryColumn(f.Column);
            }
        }

        static private void Indexes(this IDeleteExpressionRoot Delete, Table table)
        {
            foreach (var ci in table.Indices) {
                if (ci.Name != null)
                    Delete.Index(ci.Name).OnTable(table.Name);
                else
                    Delete.Index()
                          .OnTable(table.Name)
                          .OnColumns(ci.Columns.Select(c => c.ColumnName).ToArray());
            }
        }
    }
}
