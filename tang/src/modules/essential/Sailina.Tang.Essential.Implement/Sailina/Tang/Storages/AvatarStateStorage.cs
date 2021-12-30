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

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.Data;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Storages;

namespace Sailina.Tang.Storages
{
    [StroageName(AvatarBase.AvatarStateStorageName)]
    internal class AvatarStateStorage : AdoNetGrainStorageBase<AvatarStateStorage>
    {
        public AvatarStateStorage(ILogger<AvatarStateStorage> logger, IProviderRuntime providerRuntime, IOptions<AdoNetGrainStorageOptions> options, IOptions<ClusterOptions> clusterOptions, string name) : base(logger, providerRuntime, options, clusterOptions, name)
        {
        }

        protected override ValueTask ClearStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn) => throw new NotImplementedException();
        protected override ValueTask<(int id, string name)> GetConnectionInfo(string grainType, GrainReference grainReference, IGrainState grainState) => ValueTask.FromResult(((int)(grainReference.GetPrimaryKeyLong() / 1000000), "Conn_Game"));
        protected override async ValueTask<object> ReadStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            var avatarId = grainReference.GetPrimaryKeyLong();
            using (var reader = await conn.QueryMultipleAsync("call ReadAvatar(@AvatarId);", new { AvatarId = avatarId }))
            {
                var state = await AvatarState.InitializeAsync(reader);
                return state;
            }
        }

        protected override async ValueTask WriteStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, IDbConnection conn)
        {
            if (grainState.State is not AvatarState state)
                return;
            await state.WriteAsync(conn);
        }
    }
}
