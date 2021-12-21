using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreConnectionInfo : ConnectionInfo
    {
        public override IPAddress RemoteAddress { get; }
        public override int RemotePort { get; }
        public override IPAddress LocalAddress { get; }
        public override int LocalPort { get; }
        public override X509Certificate2 ClientCertificate { get; }

        public AspnetCoreConnectionInfo(Microsoft.AspNetCore.Http.ConnectionInfo connectionInfo)
        {
            RemoteAddress = connectionInfo.RemoteIpAddress;
            LocalAddress = connectionInfo.LocalIpAddress;
            LocalPort = connectionInfo.LocalPort;
            ClientCertificate = connectionInfo.ClientCertificate;
            RemotePort = connectionInfo.RemotePort;
        }
    }
}
