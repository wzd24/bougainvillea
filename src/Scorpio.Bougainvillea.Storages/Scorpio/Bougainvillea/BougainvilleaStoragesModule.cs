using System;
using System.Collections.Generic;

using Dapper;
using Dapper.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Modularity;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    public class BougainvilleaStoragesModule : ScorpioModule
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
