using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PluginCodeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        // This is a positional argument
        public PluginCodeAttribute(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ArgumentType { get; set; }

    }
}
