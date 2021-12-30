using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Configuration;
using Orleans.Providers;
using Orleans.Runtime;

using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Storages;

namespace Sailina.Tang.Storages
{
    [StroageName(ServerBase.ServerInfoStateStorageName)]
    internal class ServerInfoStateStorage : AdoNetGrainStorageBase<ServerInfoStateStorage>
    {
        public ServerInfoStateStorage(ILogger<ServerInfoStateStorage> logger, IProviderRuntime providerRuntime, IOptions<AdoNetGrainStorageOptions> options, IOptions<ClusterOptions> clusterOptions, string name) : base(logger, providerRuntime, options, clusterOptions, name)
        {

        }

        protected override ValueTask ClearStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn) => ValueTask.CompletedTask;
        protected override ValueTask<(int id, string name)> GetConnectionInfo(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.FromResult((0, "Conn_Public"));
        protected override async ValueTask<object> ReadStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            var serverId=grainReference.GetPrimaryKeyLong();
            return (await conn.GetAllAsync<ServerInfo>(new { Id = serverId }, "Servers")).FirstOrDefault();
        }

        protected override async ValueTask WriteStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            await conn.InsertOrUpdateAsync<ServerInfo>(grainState.State, "Servers");
        }
    }
}
