using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingDefinitionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void Define(IGameSettingDefinitionContext context);
    }
}
