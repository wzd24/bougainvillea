// See https://aka.ms/new-console-template for more information

using Orleans;
using Orleans.Runtime;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.AspnetCore;
using Scorpio.Modularity;

[DependsOn(typeof(AspnetCoreHostingModule), typeof(EssentialBootstrapModule))]
internal class HostModule : ScorpioModule
{
}