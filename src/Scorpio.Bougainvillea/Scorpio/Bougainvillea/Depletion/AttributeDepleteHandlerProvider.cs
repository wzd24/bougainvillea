using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.Depletion;
using Scorpio.Bougainvillea.Handler;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Depletion
{
    internal class AttributeDepletionHandlerProvider : IDepleteHandlerProvider, IScopedDependency
    {
        private readonly AttributeDepleteHandlerOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public AttributeDepletionHandlerProvider(IServiceProvider serviceProvider, IOptions<AttributeDepleteHandlerOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }
        public IDepleteHandler GetHandler(long[] depletion)
        {
            var type = _options.GetHandlerType(depletion);
            if (type==null)
            {
                throw new NotImplementedException($"Handler for depletion [{depletion.ExpandToString(",")}] not implemented.");
            }
            var handler = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, type) as IDepleteHandler;
            return handler;
        }
    }
}
