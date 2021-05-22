using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea;
using Scorpio.Modularity;

namespace Scorpio.Bougainvillea.AspnetCore
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvilleaModule))]
    public class AspnetCoreHostingModule : ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddHttpContextAccessor();
        }
    }
}
