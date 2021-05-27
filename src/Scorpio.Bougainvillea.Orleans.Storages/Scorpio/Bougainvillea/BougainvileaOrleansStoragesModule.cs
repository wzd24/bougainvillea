using System;

using Scorpio.Bougainvillea.Storages;
using Scorpio.Modularity;

namespace Scorpio.Bougainvillea.Orleans.Storages
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvileaOrleansModule),typeof(BougainvilleaStoragesModule))]
    public class BougainvileaOrleansStoragesModule:ScorpioModule
    {
    }
}
