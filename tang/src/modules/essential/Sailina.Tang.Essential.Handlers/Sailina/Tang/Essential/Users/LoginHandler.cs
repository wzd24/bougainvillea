using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Middleware;

namespace Sailina.Tang.Essential.Users
{
    internal class LoginHandler : GameHandlerBase<LoginData>
    {
        public LoginHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<IResponseMessage> ExecuteAsync(LoginData request)
        {
            throw new NotImplementedException();
        }
    }
    internal class LoginData
    {
        public string Token { get; set; }
    }

}
