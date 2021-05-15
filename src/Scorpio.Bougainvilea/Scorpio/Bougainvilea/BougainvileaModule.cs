using System;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Modularity;

namespace Scorpio.Bougainvilea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvileaAbstractionsModule))]
    public class BougainvileaModule:ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void PreConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddConventionalRegistrar<ConventionRegistrar>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.Configure<GameSettingOptions>(opts =>
            {
                opts.SettingProviders.Add<GlobalGameSettingProvider>();
            });
            context.Services.PostConfigure<MiddlewareOptions>(opt =>
            {
                opt.AddMiddleware<UserTokenMiddleware>();
                opt.AddMiddleware<ExceptionMiddleware>();
            });
        }
    }
}
