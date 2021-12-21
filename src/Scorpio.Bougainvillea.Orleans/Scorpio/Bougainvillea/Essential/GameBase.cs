using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Orleans.CodeGeneration;
using Orleans.Hosting;
using Orleans.Runtime;

using Scorpio.Bougainvillea.Essential.Dtos;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TGame"></typeparam>
    public abstract class GameBase<TGame> : GrainBase<TGame>, IGameBase
         where TGame : GameBase<TGame>
    {
        private readonly IBaseGrainProvider _baseGrainProvider;

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(GameBase.ServerListStateName, GameBase.ServerListStateStorageName)]
        public IPersistentState<ServerListState> ServerList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="baseGrainProvider"></param>
        protected GameBase(IServiceProvider serviceProvider, IBaseGrainProvider baseGrainProvider) : base(serviceProvider)
        {
            _baseGrainProvider = baseGrainProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask<IReadOnlyCollection<ServerInfo>> GetServersAsync()
        {
            return ValueTask.FromResult<IReadOnlyCollection<ServerInfo>>(ServerList.State);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask<ServerInfo> GetServerAsync(int serverId)
        {
            return ValueTask.FromResult(ServerList.State.SingleOrDefault(s => s.Id == serverId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        public async ValueTask<GenerateServerErrorCode> GenerateServerAsync(ServerInfo serverInfo)
        {
            if (serverInfo == null)
                return GenerateServerErrorCode.ArgumentNull;
            if (serverInfo.Status != ServerStatus.AwaitOpen)
            {
                return GenerateServerErrorCode.InvalidStatus;
            }
            if (ServerList.State.Any(s => s.Id == serverInfo.Id))
            {
                return GenerateServerErrorCode.ServerAlreadyExists;
            }
            if (ServerList.State.Any(s => s.Name == serverInfo.Name))
            {
                return GenerateServerErrorCode.ServerNameAlreadyExists;
            }
            ServerList.State.AddIfNotContains(serverInfo);
            await this.GetStreamAsync<ServerInfo>(0, serverInfo.Id, ServerBase.StreamSubscription).OnNextAsync(serverInfo);
            return GenerateServerErrorCode.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        async ValueTask IGameBase.OpenServerAsync(int serverId)
        {
            var info = ServerList.State.SingleOrDefault(s => s.Id == serverId);
            if (info == null)
            {
                return;
            }
            var server = GrainFactory.GetGrain<IServerBase>(info.Id);
            info.Status = await server.OpenAsync();
        }

        async ValueTask IGameBase.CloseServerAsync(int serverId)
        {
            var info = ServerList.State.SingleOrDefault(s => s.Id == serverId);
            if (info == null)
            {
                return;
            }
            var server = GrainFactory.GetGrain<IServerBase>(info.Id);
            info.Status = await server.CloseAsync();
        }

        async ValueTask IGameBase.MaintenanceServerAsync(int serverId)
        {
            var info = ServerList.State.SingleOrDefault(s => s.Id == serverId);
            if (info == null)
            {
                return;
            }
            var server = GrainFactory.GetGrain<IServerBase>(info.Id);
            info.Status = await server.MaintenanceAsync();
        }

        async ValueTask IGameBase.BeginInitializeAsync()
        {
            await ServerList.State.Where(s => s.Status != ServerStatus.Closed).ForEachAsync(async s =>
                  {
                      var server = _baseGrainProvider.GetServerBase(s.Id);
                      await server.BeginInitializeAsync();
                  });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class GameBase
    {
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        public const string ServerListStateStorageName = "ServerListStateStorage";

        /// <summary>
        /// 
        /// </summary>
        public const string ServerListStateName = "ServerListState";

    }



    /// <summary>
    /// 
    /// </summary>
    public class ServerListState : SortedSet<ServerInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        public ServerListState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public ServerListState(IComparer<ServerInfo> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public ServerListState(IEnumerable<ServerInfo> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public ServerListState(IEnumerable<ServerInfo> collection, IComparer<ServerInfo> comparer) : base(collection, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ServerListState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
