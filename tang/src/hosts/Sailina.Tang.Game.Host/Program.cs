// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Hosting;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.Essential;

internal static class Program
{
    private static async Task Main(string[] args)
    {
       await Host.CreateDefaultBuilder(args).AddScorpio<HostModule>().UseOrleans().ConfigureWebHostDefaults(builder =>
        {
            builder.UseKestrel().UseStartup<Startup>();
        }).RunConsoleAsync();
    }
    private static IHostBuilder UseOrleans(this IHostBuilder builder)
    {
        return builder.UseOrleans((context, builder) =>
        {

            var env = context.Configuration["Server:Env"];
            builder.Action(env == "local", a => a.UseLocalhostClustering())
            .Configure<GrainCollectionOptions>(opts =>
            {
                opts.CollectionAge = TimeSpan.FromDays(2);
            })
            .AddStartupTask(async (sp, ctx) =>
            {
                var str= await sp.GetRequiredService<IGrainFactory>().GetGrain<IAvatar>(1).GetAvatarNameAsync();
                Console.WriteLine(str);

            })
            .AddMemoryGrainStorageAsDefault()
            .AddMemoryGrainStorage("AvatarBaseStateStorage")
            .AddSimpleMessageStreamProvider("SMSProvider")
            .AddMemoryGrainStorage("PubSubStore")
            .ConfigureApplicationParts(c => c.AddFromDependencyContext().WithReferences());
        });
    }
}