using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scorpio.Bougainvillea.Handler;
using Scorpio.DependencyInjection;
using Scorpio.Middleware.Pipeline;
using System;

namespace Scorpio.Bougainvillea.Middleware.Pipeline
{
    /// <summary>
    /// 
    /// </summary>

    internal class GamePipelineBuilder : PipelineBuilder<IGameContext>, IGamePipelineBuilder, ITransientDependency
    {
        private readonly MiddlewareOptions _options;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="options"></param>
        public GamePipelineBuilder(IServiceProvider serviceProvider, IOptions<MiddlewareOptions> options) : base(serviceProvider)
        {
            _options = options.Value;
            _options.ApplyMiddleware(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override PipelineRequestDelegate<IGameContext> TailDelegate => async context =>
            {
                var serviceProvider = context.ApplicationServices;
                var userKey = context?.User?.Key;
                var factory = serviceProvider.GetService<IGameHandlerFactory>();
                var handler = factory.Create(context.Request.RequestCode);
                if (handler == null)
                {
                    throw new HandlerNotFoundException(context.Request.RequestCode);
                }
                await handler(context);
            };
    }
}
