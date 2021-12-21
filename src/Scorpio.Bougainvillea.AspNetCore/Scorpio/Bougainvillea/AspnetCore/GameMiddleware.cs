using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.Bougainvillea.Middleware.Pipeline;
using Scorpio.Middleware.Pipeline;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal sealed class GameMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IGameContextAccessor _gameContextAccessor;
        private readonly IGamePipelineBuilder _gamePipelineBuilder;
        private readonly AspnetCoreGameOptions _options;
        private readonly PipelineRequestDelegate<IGameContext> _pipeline;

        public GameMiddleware(RequestDelegate next, IGameContextAccessor gameContextAccessor, IOptions<AspnetCoreGameOptions> options, IGamePipelineBuilder gamePipelineBuilder)
        {
            _next = next;
            _gameContextAccessor = gameContextAccessor;
            _gamePipelineBuilder = gamePipelineBuilder;
            _pipeline = _gamePipelineBuilder.Build();
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value!;

            if (!path.Equals(_options.GameUrlPath) || httpContext.Request.Method.Equals("get",System.StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(httpContext);
                return;
            }
            var gameContext = ActivatorUtilities.CreateInstance<AspnetCoreGameContext>(httpContext.RequestServices, httpContext);
            _gameContextAccessor.GameContext = gameContext;
            await _pipeline.Invoke(gameContext);
            await gameContext.Response.FlushAsync();
        }
    }
}
