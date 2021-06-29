using System;
using System.Collections.Generic;
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

using Scorpio.Bougainvillea.AdoNet;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// Logging codes used by <see cref="Orleans.Storage.AdoNetGrainStorage"/>.
    /// </summary>
    /// <remarks> These are taken from <em>Orleans.Providers.ProviderErrorCode</em> and <em>Orleans.Providers.AzureProviderErrorCode</em>.</remarks>
    internal enum RelationalStorageProviderCodes
    {
        //These is from Orleans.Providers.ProviderErrorCode and Orleans.Providers.AzureProviderErrorCode.
        ProvidersBase = 200000,

        RelationalProviderBase = ProvidersBase + 400,
        RelationalProviderDeleteError = RelationalProviderBase + 8,
        RelationalProviderInitProvider = RelationalProviderBase + 9,
        RelationalProviderNoDeserializer = RelationalProviderBase + 10,
        RelationalProviderNoStateFound = RelationalProviderBase + 11,
        RelationalProviderClearing = RelationalProviderBase + 12,
        RelationalProviderCleared = RelationalProviderBase + 13,
        RelationalProviderReading = RelationalProviderBase + 14,
        RelationalProviderRead = RelationalProviderBase + 15,
        RelationalProviderReadError = RelationalProviderBase + 16,
        RelationalProviderWriting = RelationalProviderBase + 17,
        RelationalProviderWrote = RelationalProviderBase + 18,
        RelationalProviderWriteError = RelationalProviderBase + 19
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStorage"></typeparam>
    public abstract class AdoNetGrainStorageBase<TStorage> : GrainStorageBase<TStorage>
        where TStorage : AdoNetGrainStorageBase<TStorage>
    {

        /// <summary>
        /// 
        /// </summary>
        public AdoNetGrainStorageOptions Options { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="providerRuntime"></param>
        /// <param name="options"></param>
        /// <param name="clusterOptions"></param>
        /// <param name="name"></param>
        protected AdoNetGrainStorageBase(
            ILogger<TStorage> logger, IProviderRuntime providerRuntime, IOptions<AdoNetGrainStorageOptions> options, IOptions<ClusterOptions> clusterOptions, string name) :
            base(logger,
                 providerRuntime,
                 clusterOptions,
                 name)
        {
            Options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <returns></returns>
        public override async Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var baseGrainType = ExtractBaseClass(grainType);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderClearing, LogString("Clearing grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
            }
            try
            {
                using (var conn = DbConnectionFactory.CreateConnection(Options.Invariant, Options.ConnectionString))
                {
                    await ClearStateCoreAsync(grainType, grainReference, grainState, conn);
                }

            }
            catch (Exception ex)
            {
                Logger.Error((int)RelationalStorageProviderCodes.RelationalProviderDeleteError, LogString("Error clearing grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString(), ex.Message), ex);
                throw;
            }
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderCleared, LogString("Cleared grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected abstract Task ClearStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, System.Data.IDbConnection conn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <returns></returns>
        public override async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var baseGrainType = ExtractBaseClass(grainType);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderReading, LogString("Reading grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
            }
            try
            {
                using (var conn = DbConnectionFactory.CreateConnection(Options.Invariant, Options.ConnectionString))
                {
                    var state = await ReadStateCoreAsync(grainType, grainReference, grainState, conn);
                    var recordExists = state != null;
                    if (state==null)
                    {
                        Logger.Info((int)RelationalStorageProviderCodes.RelationalProviderNoStateFound, LogString("Null grain state read (default will be instantiated)", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
                        state = Activator.CreateInstance(grainState.Type);
                    }
                    grainState.State = state;
                    grainState.RecordExists = recordExists;
                    if (Logger.IsEnabled(LogLevel.Trace))
                    {
                        Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderRead, LogString("Read grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error((int)RelationalStorageProviderCodes.RelationalProviderReadError, LogString("Error reading grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString(), ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        protected abstract Task<object> ReadStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, System.Data.IDbConnection conn);
    }
}