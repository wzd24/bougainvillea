using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Props;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential.PropsHandlers
{
    internal class PropsHandlerProvider : IPropsHandlerProvider, IScopedDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public PropsHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IPropsHandler GetHandler(int propId)
        {
            return _serviceProvider.GetService<PropsHandler>();
        }
    }
}
