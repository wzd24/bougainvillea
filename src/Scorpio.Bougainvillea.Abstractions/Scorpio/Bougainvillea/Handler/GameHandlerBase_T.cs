using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GameHandlerBase<T> : IGameHandler, IServiceProviderAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        protected ICurrentUser CurrentUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected GameHandlerBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CurrentUser = serviceProvider.GetService<ICurrentUser>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(IGameContext context)
        {
            var ser = ServiceProvider.GetRequiredService<IJsonSerializer>();
            var request = ser.Deserialize<T>(context.Request.Content);
            var result = await ExecuteAsync(request);
            await context.Response.WriteAsync(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Task<IResponseMessage> ExecuteAsync(T request);
    }
}
