using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Configuration.Overrides;
using Orleans.Providers;
using Orleans.Storage;

namespace Scorpio.Bougainvillea.Storages
{
    internal class GrainStorageFactory
    {
        public static IGrainStorage Create<TStroage>(IServiceProvider services, string name)
            where TStroage : IGrainStorage
        {
            var clusterOptions = services.GetProviderClusterOptions(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);
            return ActivatorUtilities.CreateInstance<TStroage>(services, name, clusterOptions);
        }

        internal static IGrainStorage Create(IServiceProvider services, Type storageType, string name)
        {
            var clusterOptions = services.GetProviderClusterOptions(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME);
            return ActivatorUtilities.CreateInstance(services, storageType, name, clusterOptions) as IGrainStorage;
        }
    }
}
