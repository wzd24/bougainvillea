using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Middleware;

namespace Sailina.Tang.Essential.Users
{
    internal class EnterHandler : GameHandlerBase<EnterData>
    {
        public EnterHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<IResponseMessage> ExecuteAsync(EnterData request) => throw new NotImplementedException();
    }

    internal class EnterData
    {
        public string   Token { get; set; }

        public int ServerId { get; set; }
    }
}
