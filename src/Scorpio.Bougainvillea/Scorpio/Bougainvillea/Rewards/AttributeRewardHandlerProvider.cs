using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.Handler;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Rewards
{
    internal class AttributeRewardHandlerProvider : IRewardHandlerProvider, ISingletonDependency
    {
        private readonly AttributeRewardHandlerOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public AttributeRewardHandlerProvider(IServiceProvider serviceProvider, IOptions<AttributeRewardHandlerOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }
        public IRewardHandler GetHandler(int[] rewards)
        {
            var type = _options.GetHandlerType(rewards);
            if (type==null)
            {
                throw new NotImplementedException($"Handler for rewards [{rewards.ExpandToString(",")}] not implemented.");
            }
            var handler = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, type) as IRewardHandler;
            return handler;
        }
    }
}
