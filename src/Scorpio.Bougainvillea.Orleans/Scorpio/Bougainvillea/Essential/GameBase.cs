﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 
        /// </summary>
        [PropertyPersistentState(ServerListStateName, ServerListStateStorageName)]
        public IPersistentState<ServerListState> ServerList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected GameBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask<IReadOnlyCollection<ServerInfo>> GetServers()
        {
            return ValueTask.FromResult<IReadOnlyCollection<ServerInfo>>(ServerList.State);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask<ServerInfo> GetServer(int serverId)
        {
            return ValueTask.FromResult(ServerList.State.SingleOrDefault(s => s.ServerId == serverId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <returns></returns>
        public async ValueTask<GenerateServerErrorCode> GenerateServer(ServerInfo serverInfo)
        {
            if (serverInfo == null)
                return GenerateServerErrorCode.ArgumentNull;
            if (ServerList.State.Any(s => s.ServerId == serverInfo.ServerId))
            {
                return GenerateServerErrorCode.ServerAlreadyExists;
            }
            if (ServerList.State.Any(s => s.Name == serverInfo.Name))
            {
                return GenerateServerErrorCode.ServerNameAlreadyExists;
            }
            ServerList.State.AddIfNotContains(serverInfo);
            await this.GetStreamAsync<ServerInfo>(0, serverInfo.ServerId, ServerBase.StreamSubscription).OnNextAsync(serverInfo);
            return  GenerateServerErrorCode.Success;
        }
    }

    

    /// <summary>
    /// 
    /// </summary>
    public class ServerListState : SortedSet<ServerInfo>
    {

    }


}
