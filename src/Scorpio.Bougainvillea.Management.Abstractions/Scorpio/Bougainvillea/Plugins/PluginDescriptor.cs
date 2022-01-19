using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PluginDescriptor : IEqualityComparer<PluginDescriptor>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public PluginDescriptor(Type type)
        {
            Type = type;
            var atrr = type.GetAttribute<PluginCodeAttribute>();
            Code = atrr.Code;
            Descript = atrr.Description;
            Name = atrr.Name;
            Argument = atrr.ArgumentType != null ? JsonConvert.SerializeObject(Activator.CreateInstance(atrr.ArgumentType)) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Argument { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Descript { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(PluginDescriptor x, PluginDescriptor y) => x.Code == y.Code;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetHashCode(PluginDescriptor obj) => obj.Code.GetHashCode();
        internal IManagementPlugin Generate(IServiceProvider serviceProvider) =>
            ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, Type) as IManagementPlugin;
    }
}
