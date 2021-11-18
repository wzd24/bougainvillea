// See https://aka.ms/new-console-template for more information

using Orleans;
using Orleans.Runtime;

using Sailina.Tang.Essential;
using Sailina.Tang.Essential.Implement;

using Scorpio.Bougainvillea.AspnetCore;
using Scorpio.Modularity;

[DependsOn(typeof(AspnetCoreHostingModule), typeof(EssentialImplementModule))]
internal class HostModule : ScorpioModule
{
}