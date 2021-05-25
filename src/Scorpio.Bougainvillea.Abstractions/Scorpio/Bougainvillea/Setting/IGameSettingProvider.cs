﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public GameSettingScope Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        Task<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition<T> settingDefinition) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, int key, T value) where T : GameSettingBase;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingDefinition"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, IReadOnlyDictionary<int, T> values) where T : GameSettingBase;
    }
}
