using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingProviderManager
    {
        /// <summary>
        /// 
        /// </summary>
        ICollection<IGameSettingProvider> Providers { get; }
    }
}
