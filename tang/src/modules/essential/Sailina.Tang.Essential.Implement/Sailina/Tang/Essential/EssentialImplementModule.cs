using Dapper.Extensions;
using Dapper;

using Microsoft.Extensions.DependencyInjection;

using Sailina.Tang.Essential.BeautySystem;
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
            opts.SubSystems.Add<BeautySubSystem>();
        });
        context.Services.Configure<PropsHandleOptions>(opts => opts.HandlerProviders.Add<PropsHandlerProvider>());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Initialize(ApplicationInitializationContext context)
    {
        SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<FromType, Dictionary<int, long>>>());
        SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<int, SystemRewardInfo>>());

    }
}
