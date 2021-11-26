using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Essential
{
    internal class ServerDateTimeProvider : IDateTimeProvider,ISingletonDependency
    {
        private readonly IBaseGrainProvider _baseGrainProvider;
        private readonly ICurrentServer _currentServer;

        public ServerDateTimeProvider(IBaseGrainProvider baseGrainProvider,ICurrentServer currentServer)
        {
            _baseGrainProvider = baseGrainProvider;
            _currentServer = currentServer;
        }

        public ValueTask<DateTimeOffset> GetNowAsync() => _baseGrainProvider.GetServerBase(_currentServer.ServerId).GetServerTimeAsync();
    }
}
