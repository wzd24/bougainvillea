using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.AdoNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbConnectionFactory
    {
        private static readonly ConcurrentDictionary<string, CachedFactory> _factoryCache =
            new ConcurrentDictionary<string, CachedFactory>();

        private static readonly Dictionary<string, List<Tuple<string, string>>> _providerFactoryTypeMap =
            new Dictionary<string, List<Tuple<string, string>>>
            {
                { AdoNetInvariants.InvariantNameSqlServer, new List<Tuple<string, string>>{ new Tuple<string, string>("System.Data.SqlClient", "System.Data.SqlClient.SqlClientFactory") } },
                { AdoNetInvariants.InvariantNameMySql, new List<Tuple<string, string>>{ new Tuple<string, string>("MySql.Data", "MySql.Data.MySqlClient.MySqlClientFactory") } },
                { AdoNetInvariants.InvariantNamePostgreSql, new List<Tuple<string, string>>{ new Tuple<string, string>("Npgsql", "Npgsql.NpgsqlFactory") } },
                { AdoNetInvariants.InvariantNameSqlLite, new List<Tuple<string, string>>{ new Tuple<string, string>("Microsoft.Data.SQLite", "Microsoft.Data.SQLite.SqliteFactory") } },
                { AdoNetInvariants.InvariantNameSqlServerDotnetCore,new List<Tuple<string, string>>{ new Tuple<string, string>("Microsoft.Data.SqlClient", "Microsoft.Data.SqlClient.SqlClientFactory") } },
                { AdoNetInvariants.InvariantNameMySqlConnector, new List<Tuple<string, string>>{ new Tuple<string, string>("MySqlConnector", "MySqlConnector.MySqlConnectorFactory") , new Tuple<string, string>("MySqlConnector", "MySql.Data.MySqlClient.MySqlClientFactory") } },
            };

        private static CachedFactory GetFactory(string invariantName)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentNullException(nameof(invariantName));
            }

            if (!_providerFactoryTypeMap.TryGetValue(invariantName, out var providerFactoryDefinitions) || providerFactoryDefinitions.Count == 0)
                throw new InvalidOperationException($"Database provider factory with '{invariantName}' invariant name not supported.");

            List<Exception> exceptions = null;
            foreach (var providerFactoryDefinition in providerFactoryDefinitions)
            {
                Assembly asm = null;
                try
                {
                    var asmName = new AssemblyName(providerFactoryDefinition.Item1);
                    asm = Assembly.Load(asmName);
                }
                catch (Exception exc)
                {
                    AddException(new InvalidOperationException($"Unable to find and/or load a candidate assembly '{providerFactoryDefinition.Item1}' for '{invariantName}' invariant name.", exc));
                    continue;
                }

                if (asm == null)
                {
                    AddException(new InvalidOperationException($"Can't find database provider factory with '{invariantName}' invariant name. Please make sure that your ADO.Net provider package library is deployed with your application."));
                    continue;
                }

                var providerFactoryType = asm.GetType(providerFactoryDefinition.Item2);
                if (providerFactoryType == null)
                {
                    AddException(new InvalidOperationException($"Unable to load type '{providerFactoryDefinition.Item2}' for '{invariantName}' invariant name."));
                    continue;
                }

                var prop = providerFactoryType.GetFields().SingleOrDefault(p => string.Equals(p.Name, "Instance", StringComparison.OrdinalIgnoreCase) && p.IsStatic);
                if (prop == null)
                {
                    AddException(new InvalidOperationException($"Invalid provider type '{providerFactoryDefinition.Item2}' for '{invariantName}' invariant name."));
                    continue;
                }

                var factory = (DbProviderFactory)prop.GetValue(null);
                return new CachedFactory(factory, providerFactoryType.Name, "", providerFactoryType.AssemblyQualifiedName);
            }

            throw new AggregateException(exceptions);

            void AddException(Exception ex)
            {
                if (exceptions == null)
                {
                    exceptions = new List<Exception>();
                }
                exceptions.Add(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invariantName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection CreateConnection(string invariantName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(invariantName))
            {
                throw new ArgumentNullException(nameof(invariantName));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var factory = _factoryCache.GetOrAdd(invariantName, GetFactory).Factory;
            var connection = factory.CreateConnection();

            if (connection == null)
            {
                throw new InvalidOperationException($"Database provider factory: '{invariantName}' did not return a connection object.");
            }

            connection.ConnectionString = connectionString;
            return connection;
        }

        private class CachedFactory
        {
            public CachedFactory(DbProviderFactory factory, string factoryName, string factoryDescription, string factoryAssemblyQualifiedNameKey)
            {
                Factory = factory;
                FactoryName = factoryName;
                FactoryDescription = factoryDescription;
                FactoryAssemblyQualifiedNameKey = factoryAssemblyQualifiedNameKey;
            }

            /// <summary>
            /// The factory to provide vendor specific functionality.
            /// </summary>
            /// <remarks>For more about <see href="http://florianreischl.blogspot.fi/2011/08/adonet-connection-pooling-internals-and.html">ConnectionPool</see>
            /// and issues with using this factory. Take these notes into account when considering robustness of Orleans!</remarks>
            public readonly DbProviderFactory Factory;

            /// <summary>
            /// The name of the loaded factory, set by a database connector vendor.
            /// </summary>
            public readonly string FactoryName;

            /// <summary>
            /// The description of the loaded factory, set by a database connector vendor.
            /// </summary>
            public readonly string FactoryDescription;

            /// <summary>
            /// The description of the loaded factory, set by a database connector vendor.
            /// </summary>
            public readonly string FactoryAssemblyQualifiedNameKey;
        }
    }
}
