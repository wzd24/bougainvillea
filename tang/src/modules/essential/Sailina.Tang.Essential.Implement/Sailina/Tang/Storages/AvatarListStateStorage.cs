using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Configuration;
using Orleans.Providers;
using Orleans.Runtime;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.Data;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Storages;

namespace Sailina.Tang.Storages
{
    [StroageName(ServerBase.AvatarListStateStorageName)]
    internal class AvatarListStateStorage : AdoNetGrainStorageBase<AvatarListStateStorage>
    {
        public AvatarListStateStorage(ILogger<AvatarListStateStorage> logger, IProviderRuntime providerRuntime, IOptions<AdoNetGrainStorageOptions> options, IOptions<ClusterOptions> clusterOptions, string name) : base(logger, providerRuntime, options, clusterOptions, name)
        {
        }

        protected override ValueTask ClearStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn) => throw new NotImplementedException();
        protected override ValueTask<(int id, string name)> GetConnectionInfo(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.FromResult(((int)grainReference.GetPrimaryKeyLong(), "Conn_Game"));
        protected override async ValueTask<object> ReadStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            var serverId=(int)grainReference.GetPrimaryKeyLong();
            var values =(await conn.GetAllAsync<AvatarInfo>(new { ServerId = serverId }, "ServerAvatars"));
            values.OfType<IModifiable>().ForEach(value =>value.ResetModifyState());
            return new AvatarListState(values);
        }

        protected override async ValueTask WriteStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            if (grainState.State is not AvatarListState state)
                return;
            var values=state.Where(v=>(v as IModifiable).Modified).ToList();
          await  conn.InsertOrUpdateAsync<AvatarInfo>(values, "ServerAvatars");
        }
    }
}
