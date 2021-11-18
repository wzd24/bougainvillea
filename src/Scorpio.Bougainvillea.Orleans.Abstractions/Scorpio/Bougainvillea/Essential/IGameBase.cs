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
    public interface IGameBase:IGrainBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<IReadOnlyCollection<ServerInfo>> GetServers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ValueTask<ServerInfo> GetServer(int serverId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        ValueTask<GenerateServerErrorCode> GenerateServer(ServerInfo serverInfo);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GenerateServerErrorCode
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
        /// <summary>
        /// 
        /// </summary>
        ArgumentNull = 1,

        /// <summary>
        /// 
        /// </summary>
        ServerAlreadyExists = 2,

        /// <summary>
        /// 
        /// </summary>
        ServerNameAlreadyExists = 3
    }
}
