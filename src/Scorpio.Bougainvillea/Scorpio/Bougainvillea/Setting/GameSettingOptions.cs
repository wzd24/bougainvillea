using System.Collections.Generic;

using Scorpio;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public  class GameSettingOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ITypeList<IGameSettingDefinitionProvider> DefinitionProviders { get; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeList<IGameSettingProvider> SettingProviders { get; }

        /// <summary>
        /// 
        /// </summary>
        public GameSettingOptions()
        {
            DefinitionProviders = new TypeList<IGameSettingDefinitionProvider>();
            SettingProviders = new TypeList<IGameSettingProvider>();
        }
    }
}