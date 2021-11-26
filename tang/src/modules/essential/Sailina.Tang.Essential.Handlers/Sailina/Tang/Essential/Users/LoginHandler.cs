
using Orleans;

using Sailina.Tang.Essential.Dtos;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Tokens;

namespace Sailina.Tang.Essential.Users
{
    [Handler("G003")]
    internal class LoginHandler : GameHandlerBase<LoginData>
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IUserTokenProvider _userTokenProvider;

        public LoginHandler(IServiceProvider serviceProvider,IGrainFactory grainFactory,IUserTokenProvider userTokenProvider) : base(serviceProvider)
        {
            _grainFactory = grainFactory;
            _userTokenProvider = userTokenProvider;
        }

        protected override async Task<IResponseMessage> PreExecuteAsync(LoginData request)
        {
            await base.PreExecuteAsync(request);
            var user =_userTokenProvider.GetUserData(request.Token);
            var server=_grainFactory.GetGrain<IServer>(request.ServerId);
            var check=await server.CheckUserAsync(user);
            if (!check.Exists)
            {
                return Error(100001);
            }
            if (!check.CanLogin)
            {
                return Error(100002);
            }
            return Success(null);
        }

        protected override async Task<IResponseMessage> ExecuteAsync(LoginData request)
        {
            var server = _grainFactory.GetGrain<IServer>(request.ServerId);
            await server.BeginLoginAsync(request);
            return Success(null);
        }
    }

}
