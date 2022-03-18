using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    public class AdoNetGrainStorageOptions
    {
        /// <summary>
        /// The default ADO.NET invariant used for storage if none is given.
        /// </summary>
        public const string DEFAULT_ADONET_INVARIANT = "System.Data.SqlClient";

        /// <summary>
        /// Connection string for AdoNet storage.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The invariant name for storage.
        /// </summary>
        public string Invariant { get; set; } = "MySql.Data.MySqlClient";

        /// <summary>
        /// Stage of silo lifecycle where storage should be initialized. Storage must be initialized prior to use.
        /// </summary>
        public int InitStage { get; set; } = 10000;
    }
}
