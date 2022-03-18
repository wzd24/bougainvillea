﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EasyMigrator.Parsing.Model;
using EasyMigrator.Extensions;
using FluentMigrator.Builders;
using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Alter.Column;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Column;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.SqlServer;

namespace EasyMigrator
{
    /// <summary>
    /// 
    /// </summary>
    static public class CreateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Create"></param>
        static public void Table<T>(this ICreateExpressionRoot Create) => Create.Table(typeof(T));
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Create"></param>
        /// <param name="tableType"></param>
        static public void Table(this ICreateExpressionRoot Create, Type tableType)
        {
            var table = tableType.ParseTable().Table;
            var createTableSyntax = Create.Table(table.Name);
            foreach (var col in table.Columns)
                createTableSyntax.WithColumn(col.Name)
                                 .BuildColumn<ICreateTableColumnAsTypeSyntax,
                                              ICreateTableColumnOptionOrWithColumnSyntax,
                                              ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax>(table, col);

            var pkCreator = Create.PrimaryKey(table.PrimaryKeyName)
                                  .OnTable(table.Name)
                                  .Columns(table.Columns.PrimaryKey().Select(c => c.Name).ToArray());

            if (table.PrimaryKeyIsClustered)
                pkCreator.Clustered();
            else
                pkCreator.NonClustered();

            foreach (var col in table.Columns.Where(c => c.ForeignKey != null))
            {
                var fkadd = Create.ForeignKey(col.ForeignKey.Name).FromTable(table.Name).ForeignColumn(col.Name).ToTable(col.ForeignKey.Table).PrimaryColumn(col.ForeignKey.Column);
                if (col.ForeignKey.OnDelete.HasValue)
                    fkadd = fkadd.OnDelete(col.ForeignKey.OnDelete.Value);
                if (col.ForeignKey.OnUpdate.HasValue)
                    fkadd = fkadd.OnUpdate(col.ForeignKey.OnUpdate.Value);
            }

            foreach (var ci in table.Indices)
            {
                var ib = Create.Index(ci.Name).OnTable(tableType).OnColumns(ci.Columns.Select(c => new IndexColumn(c.ColumnName, c.Direction)).ToArray());
                if (ci.Unique)
                    ib.WithOptions().Unique();
                if (ci.Clustered)
                    ib.WithOptions().Clustered();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Create"></param>
        static public void Columns<T>(this ICreateExpressionRoot Create) => Create.Columns(typeof(T), null, null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Create"></param>
        /// <param name="Alter"></param>
        /// <param name="populate"></param>
        static public void Columns<T>(this ICreateExpressionRoot Create, IAlterExpressionRoot Alter, Action populate) => Create.Columns(typeof(T), Alter, populate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Create"></param>
        /// <param name="tableType"></param>
        static public void Columns(this ICreateExpressionRoot Create, Type tableType) => Create.Columns(tableType, null, null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Create"></param>
        /// <param name="tableType"></param>
        /// <param name="Alter"></param>
        /// <param name="populate"></param>
        static public void Columns(this ICreateExpressionRoot Create, Type tableType, IAlterExpressionRoot Alter, Action populate)
        {
            var table = tableType.ParseTable().Table;
            var nonNullables = new List<Column>();
            foreach (var col in table.Columns.DefinedInPoco())
            { // avoids trying to add the default primary key column
                if (populate != null && !col.IsNullable && col.DefaultValue == null)
                {
                    col.IsNullable = true;
                    nonNullables.Add(col);
                }

                Create.Column(col.Name).OnTable(table.Name)
                                 .BuildColumn<ICreateColumnAsTypeOrInSchemaSyntax,
                                              ICreateColumnOptionSyntax,
                                              ICreateColumnOptionOrForeignKeyCascadeSyntax>(table, col);
            }

            foreach (var col in table.Columns.Where(c => c.ForeignKey != null))
            {
                var fkadd = Create.ForeignKey(col.ForeignKey.Name).FromTable(table.Name).ForeignColumn(col.Name).ToTable(col.ForeignKey.Table).PrimaryColumn(col.ForeignKey.Column);
                if (col.ForeignKey.OnDelete.HasValue)
                    fkadd = fkadd.OnDelete(col.ForeignKey.OnDelete.Value);
                if (col.ForeignKey.OnUpdate.HasValue)
                    fkadd = fkadd.OnUpdate(col.ForeignKey.OnUpdate.Value);
            }

            foreach (var ci in table.Indices)
            {
                //if (ci.Clustered) {
                //    var ib = Create.UniqueConstraint(ci.Name).OnTable(table.Name);
                //    foreach (var col in ci.Columns) {
                //        var cb = ib.Column(col.ColumnName);
                //        if (col.Direction == SortOrder.Ascending)
                //            cb.
                //    }
                //}
                //else {
                var ib = Create.Index(ci.Name).OnTable(tableType).OnColumns(ci.Columns.Select(c => new IndexColumn(c.ColumnName, c.Direction)).ToArray());
                if (ci.Unique)
                    ib.WithOptions().Unique();
                if (ci.Clustered)
                    ib.WithOptions().Clustered();
                //}
            }

            if (populate != null)
            {
                populate();
                foreach (var col in nonNullables)
                {
                    col.IsNullable = false;
                    Alter.Column(col.Name).OnTable(table.Name)
                                     .BuildColumn<IAlterColumnAsTypeOrInSchemaSyntax,
                                                  IAlterColumnOptionSyntax,
                                                  IAlterColumnOptionOrForeignKeyCascadeSyntax>(table, col);
                }
            }
        }

        static private void BuildColumn<TSyntax, TNext, TNextFk>(this TSyntax s, Table table, Column col)
            where TSyntax : IColumnTypeSyntax<TNext>
            where TNext : IColumnOptionSyntax<TNext, TNextFk>
            where TNextFk : IColumnOptionSyntax<TNext, TNextFk>, TNext
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (col is null)
            {
                throw new ArgumentNullException(nameof(col));
            }

            var createColumnOptionSyntax = s.As<TSyntax, TNext, TNextFk>(col);

            createColumnOptionSyntax =
                col.IsNullable
                    ? createColumnOptionSyntax.Nullable()
                    : createColumnOptionSyntax.NotNullable();

            if (col.DefaultValue != null)
            {
                var defVal = col.DefaultValue;
                if (defVal.StartsWith("'"))
                    defVal = defVal.Substring(1);
                if (defVal.EndsWith("'"))
                    defVal = defVal.Substring(0, defVal.Length - 1);
                createColumnOptionSyntax = createColumnOptionSyntax.WithDefaultValue(defVal);
            }
            if (col.AutoIncrement != null)
                createColumnOptionSyntax = createColumnOptionSyntax.Identity((int)col.AutoIncrement.Seed, (int)col.AutoIncrement.Step);
        }

        static private TNext As<TSyntax, TNext, TNextFk>(this TSyntax s, Column col)
            where TSyntax : IColumnTypeSyntax<TNext>
            where TNext : IColumnOptionSyntax<TNext, TNextFk>
            where TNextFk : IColumnOptionSyntax<TNext, TNextFk>, TNext
        {
            if (col.CustomType != null)
            {
                var sb = new StringBuilder();
                sb.Append(col.CustomType);
                if (col.Length.HasValue)
                {
                    sb.Append('(');
                    if (col.Length == int.MaxValue)
                        sb.Append("MAX");
                    else
                        sb.Append(col.Length);
                    sb.Append(')');
                }
                else if (col.Precision?.Precision > 0 && col.Precision?.Scale > 0)
                {
                    sb.Append('(');
                    sb.Append(col.Precision.Precision);
                    sb.Append(',');
                    sb.Append(col.Precision.Scale);
                    sb.Append(')');
                }
                else if (col.Precision?.Precision > 0 || col.Precision?.Scale > 0)
                {
                    sb.Append('(');
                    sb.Append(Math.Max(col.Precision?.Precision ?? 0, col.Precision?.Scale ?? 0));
                    sb.Append(')');
                }

                return s.AsCustom(sb.ToString());
            }

            switch (col.Type)
            {
                case DbType.AnsiString: return col.Length.IfHasValue(s.AsAnsiString, s.AsAnsiString);
                case DbType.AnsiStringFixedLength: return s.AsFixedLengthAnsiString(col.Length.Value);
                case DbType.Binary: return col.Length.IfHasValue(s.AsBinary, s.AsBinary);
                case DbType.Byte: return s.AsByte();
                case DbType.Boolean: return s.AsBoolean();
                case DbType.Currency: return s.AsCurrency();
                case DbType.Date: return s.AsDate();
                case DbType.DateTime: return s.AsDateTime();
                case DbType.DateTime2: return col.Precision.IfNull(s.AsCustom("DATETIME2"), p => s.AsCustom($"DATETIME2({p.Precision})"));
                case DbType.DateTimeOffset: return col.Precision.IfNull(s.AsDateTimeOffset(), p => s.AsCustom($"DATETIMEOFFSET({p.Precision})"));
                case DbType.Decimal: return col.Precision.IfNotNull(p => s.AsDecimal(p.Precision, p.Scale), s.AsDecimal);
                case DbType.Double: return s.AsDouble();
                case DbType.Guid: return s.AsGuid();
                case DbType.Int16: return s.AsInt16();
                case DbType.Int32: return s.AsInt32();
                case DbType.Int64: return s.AsInt64();
                //case DbType.Object: return s.AsByte(); // TODO: Use AsCustom
                case DbType.SByte: return s.AsByte(); // TODO: signed byte not supported directly by FluentMigrator
                case DbType.Single: return s.AsFloat();
                case DbType.String: return col.Length.IfHasValue(s.AsString, s.AsString);
                case DbType.StringFixedLength: return s.AsFixedLengthString(col.Length.Value);
                case DbType.Time: return s.AsTime();
                case DbType.UInt16: return s.AsInt16(); // TODO: unsigned integers not supported directly by FluentMigrator
                case DbType.UInt32: return s.AsInt32();
                case DbType.UInt64: return s.AsInt64();
                //case DbType.VarNumeric: return s.AsByte();
                case DbType.Xml: return col.Length.IfHasValue(s.AsXml, s.AsXml);
                default: throw new Exception("DbType '" + col.Type + "' is not mapped to a column type for FluentMigrator.");
            }
        }
    }
}
