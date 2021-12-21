using Scorpio.Bougainvillea.Middleware.Pipeline;
using Scorpio.Middleware.Pipeline;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class MiddlewareOptions
    {
        private readonly List<Action<IGamePipelineBuilder>> _middlewares;

        /// <summary>
        /// 
        /// </summary>
        public MiddlewareOptions()
        {
            _middlewares = new List<Action<IGamePipelineBuilder>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="middleware"></param>
        public void AddMiddleware(Func<IGameContext, Func<Task>, Task> middleware)
        {
            _middlewares.Add(app => app.Use(next =>
             {
                 return context =>
                 {
                     Func<Task> simpleNext = () => next(context);
                     return middleware(context, simpleNext);
                 };
             }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMiddleware"></typeparam>
        public void AddMiddleware<TMiddleware>()
        {
            AddMiddleware(typeof(TMiddleware));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="middlewareType"></param>
        /// <param name="args"></param>
        public void AddMiddleware(Type middlewareType, params object[] args)
        {
            _middlewares.Add(builder => builder.UseMiddleware(middlewareType, args));
        }

        internal void ApplyMiddleware(IGamePipelineBuilder builder)
        {
            foreach (var item in _middlewares)
            {
                item(builder);
            }
        }
    }
}
