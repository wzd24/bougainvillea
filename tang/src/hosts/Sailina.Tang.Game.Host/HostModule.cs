﻿// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;

using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using Orleans.Serialization;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.AspnetCore;
using Scorpio.Modularity;


[DependsOn(typeof(AspnetCoreHostingModule), typeof(EssentialBootstrapModule))]
internal class HostModule : ScorpioModule
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        base.ConfigureServices(context);
    }
}