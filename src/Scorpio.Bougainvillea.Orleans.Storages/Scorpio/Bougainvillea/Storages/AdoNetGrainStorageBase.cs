using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Configuration;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;

using Scorpio.Bougainvillea.AdoNet;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// Logging codes used by <see cref="AdoNetGrainStorageBase{TStorage}"/>.
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
        public IDbConnectionFactory DbConnectionFactory { get; }

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
            DbConnectionFactory = providerRuntime.ServiceProvider.GetService<IDbConnectionFactory>();
            Options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <returns></returns>
        protected abstract ValueTask<(int id, string name)> GetConnectionInfo(string grainType, GrainReference grainReference, IGrainState grainState);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override int GetSubscribeStage() => Options.InitStage;
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
                var connectionInfo = await GetConnectionInfo(grainType, grainReference, grainState);
                using (var conn = await DbConnectionFactory.GetDbConnectionAsync(connectionInfo.id, connectionInfo.name))
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
        protected abstract ValueTask ClearStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, System.Data.IDbConnection conn);

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
                var connectionInfo = await GetConnectionInfo(grainType, grainReference, grainState);
                using (var conn = await DbConnectionFactory.GetDbConnectionAsync(connectionInfo.id, connectionInfo.name))
                {
                    var state = await ReadStateCoreAsync(grainType, grainReference, grainState, conn);
                    var recordExists = state != null;
                    if (state == null)
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
        protected abstract ValueTask<object> ReadStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, System.Data.IDbConnection conn);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <returns></returns>
        public override async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            var baseGrainType = ExtractBaseClass(grainType);
            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderWriting, LogString("Writing grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
            }
            var connectionInfo = await GetConnectionInfo(grainType, grainReference, grainState);
            try
            {
                using (var conn = await DbConnectionFactory.GetDbConnectionAsync(connectionInfo.id, connectionInfo.name))
                {
                    await WriteStateCoreAsync(grainType, grainReference, grainState, conn);
                }
            }
            catch (Exception ex)
            {
                Logger.Error((int)RelationalStorageProviderCodes.RelationalProviderWriteError, LogString("Error writing grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString(), ex.Message), ex);
                throw;
            }
            //No errors found, the version of the state held by the grain can be updated.
            grainState.RecordExists = true;

            if (Logger.IsEnabled(LogLevel.Trace))
            {
                Logger.Trace((int)RelationalStorageProviderCodes.RelationalProviderWrote, LogString("Wrote grain state", ServiceId, Name, grainState.ETag, baseGrainType, grainReference.ToKeyString()));
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
        protected abstract ValueTask WriteStateCoreAsync(string grainType, GrainReference grainReference, IGrainState grainState, System.Data.IDbConnection conn);

    }
}