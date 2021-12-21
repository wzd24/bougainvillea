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
            Code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; }

    }
}
