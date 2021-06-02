using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public static class SettingDefinitionContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="descript"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ISettingDefinitionContext Add<T>(this ISettingDefinitionContext context, string name,string displayName=default,string descript=default, T defaultValue=default)
        {
            var definition = new SettingDefinition<T>(name, displayName, descript, defaultValue);
            context.Add(definition);
            return context;
        }
    }
}
