﻿using System;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GameSettingDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        public GameSettingScope Scope { get; }
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///  brief description for this setting.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GenerateScript { get; set; }

        /// <summary>
        /// Value type of the setting.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Default value of the setting.
        /// </summary>
        public object Default { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="valueType"></param>
        /// <param name="defaultValue"></param>
        protected GameSettingDefinition(string name,GameSettingScope scope, string displayName, string description, Type valueType, object defaultValue)
        {
            Name = name;
            Scope = scope;
            DisplayName = string.IsNullOrEmpty(displayName) ? name : displayName;
            Description = description;
            ValueType = valueType;
            Default = defaultValue;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameSettingDefinition<T> : GameSettingDefinition where T : class
    {

        /// <summary>
        /// 
        /// </summary>
        public GameSettingDefinition(GameSettingScope scope, T defaultValue = default) : this(typeof(T).Name,scope, null, defaultValue: defaultValue)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="defaultValue"></param>
        public GameSettingDefinition(string name, GameSettingScope scope, T defaultValue = default) : this(name,scope, null, defaultValue: defaultValue)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        public GameSettingDefinition(string name, GameSettingScope scope, string displayName, T defaultValue = default) : this(name,scope, displayName, null, defaultValue)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="defaultValue"></param>
        public GameSettingDefinition(string name, GameSettingScope scope, string displayName, string description, T defaultValue = default)
            : base(name,scope, displayName, description, typeof(T), defaultValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public new T Default => (T)base.Default;

    }

    /// <summary>
    /// 
    /// </summary>
    public class GameSettingDefaultionCustom : GameSettingDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingType"></param>
        /// <param name="scope"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="defaultValue"></param>
        public GameSettingDefaultionCustom(Type settingType, GameSettingScope scope, string displayName = null, string description = null, object defaultValue = null) 
            : base(settingType.Name, scope, displayName, description, settingType, defaultValue)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GameSettingScope
    {
        /// <summary>
        /// 
        /// </summary>
        Default,

        /// <summary>
        /// 
        /// </summary>
        Global,

        /// <summary>
        /// 
        /// </summary>
        Server
    }
}