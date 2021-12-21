using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingDefinitionManager
    {
        /// <summary>
        /// Gets the <see cref="GameSettingDefinition"/> object with given unique name.
        /// Throws exception if can not find the setting.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>The <see cref="GameSettingDefinition"/> object.</returns>
        GameSettingDefinition Get(string name);

        /// <summary>
        /// Gets a list of all setting definitions.
        /// </summary>
        /// <returns>All settings.</returns>
        IReadOnlyList<GameSettingDefinition> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        GameSettingDefinition<T> Get<T>() where T : GameSettingBase;
    }
}
