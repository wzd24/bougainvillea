using Microsoft.Extensions.DependencyInjection;

using Sailina.Tang.Essential.HeroSystem;
using Sailina.Tang.Essential.PropsHandlers;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Handler;
using Scorpio.Modularity;

namespace Sailina.Tang.Essential;
/// <summary>
/// 
/// </summary>
[DependsOn(typeof(BougainvileaOrleansModule), typeof(EssentialModule))]
public class EssentialImplementModule:ScorpioModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.Configure<AvatarOptions>(opts =>
        {
            opts.SubSystems.Add<PropsSubSystem>();
            opts.SubSystems.Add<HeroSubSystem>();
        });
        context.Services.Configure<PropsHandleOptions>(opts => opts.HandlerProviders.Add<PropsHandlerProvider>());
    }
}
