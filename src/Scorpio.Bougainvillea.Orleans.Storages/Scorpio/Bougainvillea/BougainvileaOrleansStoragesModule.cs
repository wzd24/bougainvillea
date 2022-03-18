using System;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Storages;
using Scorpio.Modularity;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(BougainvilleaStoragesModule))]
    public class BougainvileaOrleansStoragesModule:ScorpioModule
    {
        
    }
}
