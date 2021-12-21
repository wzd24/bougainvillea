using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Linq;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Props;
using Scorpio.Threading;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PropsHandleOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ITypeList<IPropsHandlerProvider> HandlerProviders { get; }

        /// <summary>
        /// 
        /// </summary>
        public PropsHandleOptions()
        {
            HandlerProviders = new TypeList<IPropsHandlerProvider>();
        }
    }
}
