using System;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Setting;
using Scorpio.EventBus;
using Scorpio.Modularity;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
        typeof(BougainvilleaModule),
        typeof(BougainvileaOrleansAbstractionsModule), 
        typeof(EventBusModule),
        typeof(BougainvileaOrleansStoragesModule))]
    public class BougainvileaOrleansModule : ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ConfigureServicesContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.Configure<GameSettingOptions>(opts =>
            {
                opts.SettingProviders.Add<GlobalGrainGameSettingProvider>();
                opts.SettingProviders.Add<ServerGrainGameSettingProvider>();
            });
            context.Services.ReplaceScoped<IEventBus, GrainEventBus>();

            base.ConfigureServices(context);
        }
    }
}
