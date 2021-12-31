using System;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea.Storages;
using Scorpio.Modularity;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvilleaAbstractionsModule))]
    [DependsOn(typeof(BougainvilleaStoragesModule))]
    [DependsOn(typeof(SettingModule))]
    public class BougainvilleaModule:ScorpioModule
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
            context.Services.Configure<SettingOptions>(opts =>
            {
                opts.SettingProviders.Add<GlobalSettingProvider>();
                opts.SettingProviders.Add<ServerSettingProvider>();
            });
            context.Services.PostConfigure<MiddlewareOptions>(opt =>
            {
                opt.AddMiddleware<UserTokenMiddleware>();
                opt.AddMiddleware<ExceptionMiddleware>();
            });
        }
    }
}
