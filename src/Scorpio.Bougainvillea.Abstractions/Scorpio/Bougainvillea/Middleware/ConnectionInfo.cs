using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ConnectionInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract IPAddress RemoteAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int RemotePort { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract IPAddress LocalAddress { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract int LocalPort { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract X509Certificate2 ClientCertificate { get; }
    }
}
