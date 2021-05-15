using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvilea;
using Scorpio.Modularity;

namespace Scorpio.Bougainvillea.AspnetCore
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvileaModule))]
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
