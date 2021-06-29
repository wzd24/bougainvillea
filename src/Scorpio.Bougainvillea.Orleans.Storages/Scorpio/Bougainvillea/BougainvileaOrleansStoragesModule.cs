using System;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Storages;
using Scorpio.Modularity;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvileaOrleansModule),typeof(BougainvilleaStoragesModule))]
    public class BougainvileaOrleansStoragesModule:ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void PreConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddConventionalRegistrar<ConventionRegistrar>();
        }
    }
}
