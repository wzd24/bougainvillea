using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServerBase : IGrainWithIntegerKey, IGrainBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> CloseAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> MaintenanceAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> OpenAsync();
    }
}
