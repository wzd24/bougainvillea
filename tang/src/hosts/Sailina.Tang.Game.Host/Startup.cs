// See https://aka.ms/new-console-template for more information

using Scorpio.Bougainvillea.AspnetCore;
/// <summary>
/// 
/// </summary>
public class Startup
{
    /// <summary>
    ///  This method gets called by the runtime. Use this method to add services to the container.
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AspnetCoreGameOptions>(options =>
        {
            options.GameUrlPath = "/game";
        });
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseGameEndPoint();
        app.UseWelcomePage();

    }
}