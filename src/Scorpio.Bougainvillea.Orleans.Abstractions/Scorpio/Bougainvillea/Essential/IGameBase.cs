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
    public interface IGameBase : IGrainWithGuidKey,IGrainBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<IReadOnlyCollection<ServerInfo>> GetServersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask BeginInitializeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ValueTask<ServerInfo> GetServerAsync(int serverId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        ValueTask<GenerateServerErrorCode> GenerateServerAsync(ServerInfo serverInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ValueTask OpenServerAsync(int serverId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ValueTask CloseServerAsync(int serverId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        ValueTask MaintenanceServerAsync(int serverId);

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
        ServerNameAlreadyExists = 3,

        /// <summary>
        /// 
        /// </summary>
        InvalidStatus = 4
    }
}
