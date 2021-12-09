using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Concurrency;
using Orleans.Streams;

using Sailina.Tang.Essential.Dtos;
using Sailina.Tang.Essential.StreamDatas;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Essential.Dtos.Servers;
using Scorpio.Bougainvillea.Tokens;

namespace Sailina.Tang.Essential
{
    internal class Server : ServerBase<Server>, IServer
    {
        public Server(IServiceProvider serviceProvider, IDateTimeProvider dateTimeProvider, IUserTokenProvider userTokenProvider) : base(serviceProvider, dateTimeProvider, userTokenProvider)
        {
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            await base.OnDeactivateAsync();
        }
    }
}
