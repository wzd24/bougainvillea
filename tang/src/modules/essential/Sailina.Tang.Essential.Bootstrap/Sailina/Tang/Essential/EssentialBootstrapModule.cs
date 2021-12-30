
using FluentMigrator.Runner;

using Microsoft.Extensions.DependencyInjection;

using Sailina.Tang.Databases;

using Scorpio.Bougainvillea;
using Scorpio.Modularity;

namespace Sailina.Tang.Essential;
/// <summary>
/// 
/// </summary>
[DependsOn(typeof(EssentialImplementModule),typeof(EssentialHandlersModule))]
public class EssentialBootstrapModule : ScorpioModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddFluentMigratorCore().ConfigureRunner(builder =>
        {
            builder.AddMySql5().WithGlobalConnectionString("Conn_Game").ScanIn(typeof(EssentialMigration).Assembly).For.Migrations();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Initialize(ApplicationInitializationContext context)
    {
        context.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
    }
}
