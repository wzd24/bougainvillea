using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingStoreContext
    {
        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// 
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        GameSettingDefinition SettingDefinition { get; }
    }
}