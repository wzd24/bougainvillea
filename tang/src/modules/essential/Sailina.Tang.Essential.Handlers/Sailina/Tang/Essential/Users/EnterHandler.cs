using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Orleans;

using Sailina.Tang.Essential.Avatars;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Tokens;

using ErrorCode = Sailina.Tang.Essential.Avatars.ErrorCode;

namespace Sailina.Tang.Essential.Users
{
    [Handler("G001")]
    internal class EnterHandler : GameHandlerBase<EnterData>
    {
        private readonly IUserTokenProvider _userTokenProvider;
        private readonly IGrainFactory _grainFactory;

        public EnterHandler(IServiceProvider serviceProvider, IUserTokenProvider userTokenProvider,IGrainFactory grainFactory) : base(serviceProvider)
        {
            _userTokenProvider = userTokenProvider;
            _grainFactory = grainFactory;
        }

        protected override async Task<IResponseMessage> ExecuteAsync(EnterData request)
        {
            var server=_grainFactory.GetGrain<IServer>(request.ServerId);
            var userData=_userTokenProvider.GetUserData<UserData>(request.Token);
            if (userData == null)
            {
                return Error((int)ErrorCode.TokenVerifyFail);
            }
            var result =await server.CheckUserAsync(userData);
            return Success(result);
        }
    }

    internal class EnterData
    {
        public string Token { get; set; }

        public int ServerId { get; set; }
    }
}
