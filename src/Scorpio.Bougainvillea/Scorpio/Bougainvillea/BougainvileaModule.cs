using System;
using System.Collections.Generic;

using Dapper.Extensions;

using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Modularity;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvilleaAbstractionsModule))]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(ApplicationInitializationContext context)
        {
            SqlMapper.AddTypeHandler(new JsonConvertHandler<List<int>>());
            SqlMapper.AddTypeHandler(new JsonConvertHandler<List<long>>());
            SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<int, int>>());
            SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<int, long>>());
            SqlMapper.AddTypeHandler(new FuncConvertHandler<TimeSpan>(v => TimeSpan.FromSeconds(v.To<long>()), (p, t) => (long)t.Seconds));
            base.Initialize(context);
        }
    }
}
