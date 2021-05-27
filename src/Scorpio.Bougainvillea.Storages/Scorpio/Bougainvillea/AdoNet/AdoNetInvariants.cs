using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.AdoNet
{
    /// <summary>
    /// A holder for well known, vendor specific connector class invariant names.
    /// </summary>
    internal static class AdoNetInvariants
    {
        /// <summary>
        /// A list of the supported invariants.
        /// </summary>
        /// <remarks>The invariant names here do not match the namespaces as is often the convention.
        /// Current exception is MySQL Connector library that uses the same invariant as MySQL compared
        /// to the official Oracle distribution.</remarks>
        public static ICollection<string> Invariants { get; } = new Collection<string>(new List<string>(new[]
        {
            InvariantNameMySql,
            InvariantNamePostgreSql,
            InvariantNameSqlLite,
            InvariantNameSqlServer,
            InvariantNameSqlServerDotnetCore,
            InvariantNameMySqlConnector
        }));

        /// <summary>
        /// Microsoft SQL Server invariant name string.
        /// </summary>
        public const string InvariantNameSqlServer = "System.Data.SqlClient";

        /// <summary>
        /// SQLite invariant name string.
        /// </summary>
        public const string InvariantNameSqlLite = "System.Data.SQLite";

        /// <summary>
        /// MySql invariant name string.
        /// </summary>
        public const string InvariantNameMySql = "MySql.Data.MySqlClient";

        /// <summary>
        /// PostgreSQL invariant name string.
        /// </summary>
        public const string InvariantNamePostgreSql = "Npgsql";

        /// <summary>
        /// Dotnet core Microsoft SQL Server invariant name string.
        /// </summary>
        public const string InvariantNameSqlServerDotnetCore = "Microsoft.Data.SqlClient";

        /// <summary>
        /// An open source implementation of the MySQL connector library.
        /// </summary>
        public const string InvariantNameMySqlConnector = "MySql.Data.MySqlConnector";
    }
}
