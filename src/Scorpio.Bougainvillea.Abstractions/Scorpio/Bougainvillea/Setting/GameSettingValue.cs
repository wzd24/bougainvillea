using System;
using System.Collections;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class GameSettingValue
    {
        /// <summary>
        /// 
        /// </summary>
        public GameSettingDefinition Definition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable Value { get; }



    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class GameSettingValue<T> : GameSettingValue where T : GameSettingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public GameSettingValue(IReadOnlyCollection<T> value)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public override IReadOnlyCollection<T> Value { get; }
    }
}