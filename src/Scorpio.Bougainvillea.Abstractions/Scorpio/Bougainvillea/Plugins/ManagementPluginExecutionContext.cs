using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;


namespace Scorpio.Bougainvillea.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ManagementPluginExecutionContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grainFactory"></param>
        /// <param name="data"></param>
        /// <param name="serviceProvider"></param>
        public ManagementPluginExecutionContext(JToken data, IServiceProvider serviceProvider)
        {
            Data = data;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }


        /// <summary>
        /// 
        /// </summary>
        public JToken Data { get; }
    }
}
