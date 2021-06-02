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
        async Task IGameHandler.ExecuteAsync(IGameContext context)
        {
            var ser = ServiceProvider.GetRequiredService<IJsonSerializer>();
            var request = ser.Deserialize<T>(context.Request.Content);
            var result = await PreExecuteAsync(request);
            if (result.IsSuccessed())
            {
                result =await ExecuteAsync(request);
            }
            await context.Response.WriteAsync(result);
            if (result.IsSuccessed())
            {
               await PostExecuteAsync(context);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual Task<IResponseMessage> PreExecuteAsync(T request)
        {
            return Task.FromResult<IResponseMessage>(ResponseMessage.Sucess);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task PostExecuteAsync(IGameContext context)
        {
            return Task.CompletedTask;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Task<IResponseMessage> ExecuteAsync(T request);
    }
}
