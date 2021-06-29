using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans.Storage;
using Orleans;
using Orleans.Runtime;
using Scorpio.Timing;
using System.Xml.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Providers;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStorage"></typeparam>
    public abstract class GrainStorageBase<TStorage> : IGrainStorage, ILifecycleParticipant<ISiloLifecycle>
           where TStorage : GrainStorageBase<TStorage>
    {

        /// <summary>
        /// These chars are delimiters when used to extract a class base type from a class
        /// that is either <see cref="Type.AssemblyQualifiedName"/> or <see cref="Type.FullName"/>.
        /// <see cref="ExtractBaseClass(string)"/>.
        /// </summary>
        private static char[] BaseClassExtractionSplitDelimeters { get; } = new[] { '[', ']' };

        /// <summary>
        /// 
        /// </summary>
        protected string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        protected IProviderRuntime ProviderRuntime { get; }

        /// <summary>
        /// 
        /// </summary>
        protected string ServiceId { get; }

        /// <summary>
        /// 
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="providerRuntime"></param>
        /// <param name="clusterOptions"></param>
        /// <param name="name"></param>
        protected GrainStorageBase(
            ILogger<TStorage> logger,
            IProviderRuntime providerRuntime,
            IOptions<ClusterOptions> clusterOptions,
            string name)
        {
            ProviderRuntime = providerRuntime;
            Name = name;
            Logger = logger;
            ServiceId = clusterOptions.Value.ServiceId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifecycle"></param>
        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe(OptionFormattingUtilities.Name<TStorage>(Name), GetSubscribeStage(), Init, Close);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainType"></param>
        /// <param name="grainReference"></param>
        /// <param name="grainState"></param>
        /// <returns></returns>
        public abstract Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);


        /// <summary> Read state data function for this storage provider.</summary>
        /// <see cref="IGrainStorage.ReadStateAsync(string, GrainReference, IGrainState)"/>.
        public abstract Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);


        /// <summary> Write state data function for this storage provider.</summary>
        /// <see cref="IGrainStorage.WriteStateAsync"/>
        public abstract Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract int GetSubscribeStage();

        /// <summary> Initialization function for this storage provider. </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task Init(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        /// Close this provider
        /// </summary>
        protected virtual Task Close(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Extracts a base class from a string that is either <see cref="Type.AssemblyQualifiedName"/> or
        /// <see cref="Type.FullName"/> or returns the one given as a parameter if no type is given.
        /// </summary>
        /// <param name="typeName">The base class name to give.</param>
        /// <returns>The extracted base class or the one given as a parameter if it didn't have a generic part.</returns>
        protected virtual string ExtractBaseClass(string typeName)
        {
            var genericPosition = typeName.IndexOf("`", StringComparison.OrdinalIgnoreCase);
            if (genericPosition != -1)
            {
                //The following relies the generic argument list to be in form as described
                //at https://msdn.microsoft.com/en-us/library/w3f99sx1.aspx.
                var split = typeName.Split(BaseClassExtractionSplitDelimeters, StringSplitOptions.RemoveEmptyEntries);
                var stripped = new Queue<string>(split.Where(i => i.Length > 1 && i[0] != ',').Select(WithoutAssemblyVersion));

                return ReformatClassName(stripped);
            }
            if (typeName.IndexOf(',') < 0)
            {
                return typeName;
            }
            var typeNames = typeName.Split(',');
            return $"{typeNames.First()}.{string.Join('.', typeNames.Skip(1).Select(n => n.Substring(n.LastIndexOf('.') + 1)))}";

            string WithoutAssemblyVersion(string input)
            {
                var asmNameIndex = input.IndexOf(',');
                if (asmNameIndex >= 0)
                {
                    var asmVersionIndex = input.IndexOf(',', asmNameIndex + 1);
                    if (asmVersionIndex >= 0) return input.Substring(0, asmVersionIndex);
                    return input.Substring(0, asmNameIndex);
                }
                return input;
            }

            string ReformatClassName(Queue<string> segments)
            {
                var simpleTypeName = segments.Dequeue();
                var arity = GetGenericArity(simpleTypeName);
                if (arity <= 0) return simpleTypeName;

                var args = new List<string>(arity);
                for (var i = 0; i < arity; i++)
                {
                    args.Add(ReformatClassName(segments));
                }

                return $"{simpleTypeName}[{string.Join(",", args.Select(arg => $"[{arg}]"))}]";
            }

            int GetGenericArity(string input)
            {
                var arityIndex = input.IndexOf("`", StringComparison.OrdinalIgnoreCase);
                if (arityIndex != -1)
                {
                    return int.Parse(input.Substring(arityIndex + 1));
                }

                return 0;
            }
        }

        /// <summary>
        /// Writes a consistent log message from the given parameters.
        /// </summary>
        /// <param name="operationProlog">A free form prolog information to log.</param>
        /// <param name="serviceId">Service Id.</param>
        /// <param name="providerName">The name of this storage provider.</param>
        /// <param name="version">The grain version.</param>
        /// <param name="normalizedGrainType">Grain type without generics information.</param>
        /// <param name="grainId">The grain ID.</param>
        /// <param name="exceptionMessage">An optional exception message information to log.</param>
        /// <returns>A log string to be printed.</returns>
        protected string LogString(string operationProlog, string serviceId, string providerName, string version, string normalizedGrainType, string grainId, string exceptionMessage = null)
        {
            const string Exception = " Exception=";
            return $"{operationProlog}: ServiceId={serviceId} ProviderName={providerName} GrainType={normalizedGrainType} GrainId={grainId} ETag={version}{(exceptionMessage != null ? Exception + exceptionMessage : string.Empty)}.";
        }

    }
}
