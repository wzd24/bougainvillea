using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameSettingDefinitionContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IGameSettingDefinitionContext Add<T>(this IGameSettingDefinitionContext context, GameSettingScope scope, T defaultValue = default) where T : GameSettingBase
        {
            context.Add(new GameSettingDefinition<T>(scope, defaultValue));
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static IGameSettingDefinitionContext Add<T>(this IGameSettingDefinitionContext context, string name, GameSettingScope scope, T defaultValue = default) where T : GameSettingBase
        {
            context.Add(new GameSettingDefinition<T>(name,scope, defaultValue));
            return context;
        }
    }
}
