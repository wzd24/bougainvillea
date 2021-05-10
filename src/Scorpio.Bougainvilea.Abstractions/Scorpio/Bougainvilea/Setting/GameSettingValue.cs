using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameSettingValue
    {
        /// <summary>
        /// 
        /// </summary>
        public GameSettingDefinition Definition { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GameSettingValue<T>: GameSettingValue where T : class
    {

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyDictionary<string, T> Value { get; set; }
    }
}