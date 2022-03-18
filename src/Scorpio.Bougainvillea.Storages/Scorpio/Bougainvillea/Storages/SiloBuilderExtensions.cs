using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;
using Orleans.Runtime;
using Orleans.Storage;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    public static class SiloBuilderExtensions
    {


        /// <summary>
        /// Configure silo to use RabbitMQ grain storage for grain storage. Instructions on configuring your database are available at <see href="http://aka.ms/orleans-sql-scripts"/>.
        /// </summary>
        /// <remarks>
        /// Instructions on configuring your database are available at <see href="http://aka.ms/orleans-sql-scripts"/>.
        /// </remarks>
        public static IServiceCollection AddGrainStorage<TStorage>(this IServiceCollection services, string name)
            where TStorage : IGrainStorage
        {
            services.AddSingletonNamedService(name, (sp, name) => GrainStorageFactory.Create<TStorage>(sp, name))
                           .AddSingletonNamedService(name, (s, n) => (ILifecycleParticipant<ISiloLifecycle>)s.GetRequiredServiceByName<IGrainStorage>(n));
            return services;
        }

        /// <summary>
        /// Configure silo to use RabbitMQ grain storage for grain storage. Instructions on configuring your database are available at <see href="http://aka.ms/orleans-sql-scripts"/>.
        /// </summary>
        /// <remarks>
        /// Instructions on configuring your database are available at <see href="http://aka.ms/orleans-sql-scripts"/>.
        /// </remarks>
        public static IServiceCollection AddGrainStorage(this IServiceCollection services, Type storageType, string name)
        {
            services.AddSingletonNamedService(name, (sp, name) => GrainStorageFactory.Create(sp, storageType, name))
                           .AddSingletonNamedService(name, (s, n) => (ILifecycleParticipant<ISiloLifecycle>)s.GetRequiredServiceByName<IGrainStorage>(n));
            return services;
        }

    }
}
