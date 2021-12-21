using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Sailina.Tang.Essential.Avatars;
using Sailina.Tang.Essential.Dtos;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Tokens;

using ErrorCode = Sailina.Tang.Essential.Avatars.ErrorCode;

namespace Sailina.Tang.Essential.Users
{
    [Handler("G002")]
    internal class RegisterHandler : GameHandlerBase<RegisterData>
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IUserTokenProvider _userTokenProvider;
        private readonly IGameContextAccessor _gameContextAccessor;

        public RegisterHandler(IServiceProvider serviceProvider, IGameContextAccessor gameContextAccessor, IGrainFactory grainFactory, IUserTokenProvider userTokenProvider) : base(serviceProvider)
        {
            _gameContextAccessor = gameContextAccessor;
            _grainFactory = grainFactory;
            _userTokenProvider = userTokenProvider;
        }

        protected override async Task<IResponseMessage> PreExecuteAsync(RegisterData request)
        {
            await base.PreExecuteAsync(request);
            var user = _userTokenProvider.GetUserData<UserData>(request.Token);
            var server = _grainFactory.GetGrain<IServer>(request.ServerId);
            var check = await server.CheckUserAsync(user);
            if (check.Exists)
            {
                return Error((int)ErrorCode.CharactersAlreadyExist);
            }
            if (!check.CanRegister)
            {
                return Error((int)ErrorCode.CannotRegister);
            }
            request.UserId = user.UserId;
            request.AccountId = user.AccountId;
            request.RegisterIp = _gameContextAccessor.GameContext.ConnectionInfo.RemoteAddress.ToString();
            return Success();
        }

        protected override async Task<IResponseMessage> ExecuteAsync(RegisterData request)
        {
            var server = _grainFactory.GetGrain<IServer>(request.ServerId);
            var code = await server.GenerateAvatarAsync(request);
            return Response(code);
        }
    }

    [Handler("G002R")]
    internal class RegisterResponseHandler : GameHandlerBase<EnterData>
    {
        private readonly IGrainFactory _grainFactory;
        private readonly IUserTokenProvider _userTokenProvider;
        private readonly IGameContextAccessor _gameContextAccessor;
        private UserData _user;

        public RegisterResponseHandler(IServiceProvider serviceProvider, IGameContextAccessor gameContextAccessor, IGrainFactory grainFactory, IUserTokenProvider userTokenProvider) : base(serviceProvider)
        {
            _gameContextAccessor = gameContextAccessor;
            _grainFactory = grainFactory;
            _userTokenProvider = userTokenProvider;
        }

        protected override async Task<IResponseMessage> PreExecuteAsync(EnterData request)
        {
            await base.PreExecuteAsync(request);
            _user = _userTokenProvider.GetUserData<UserData>(request.Token);
            var server = _grainFactory.GetGrain<IServer>(request.ServerId);
            var check = await server.CheckUserAsync(_user);
            if (check.Exists)
            {
                return Error((int)ErrorCode.CharactersAlreadyExist);
            }
            if (!check.CanRegister)
            {
                return Error((int)ErrorCode.CannotRegister);
            }
            return Success();
        }

        protected override async Task<IResponseMessage> ExecuteAsync(EnterData request)
        {
            var server = _grainFactory.GetGrain<IServer>(request.ServerId);
            var (code, result) = await server.EndGenerateAvatarAsync(_user.UserId);
            if (code == 0 && result.Status == Scorpio.Bougainvillea.Essential.AvatarInfoStatus.OnLine)
            {
                var avatar = _grainFactory.GetGrain<IAvatar>(result.AvatarId);
                return Success(new { result.Status, Info = await avatar.GetLoginInfoAsync() });
            }
            return Response(code, data: new { result?.Status });
        }
    }
}
