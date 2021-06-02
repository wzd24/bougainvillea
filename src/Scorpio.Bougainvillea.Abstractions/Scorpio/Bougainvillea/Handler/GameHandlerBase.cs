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
    public abstract class GameHandlerBase : IGameHandler, IServiceProviderAccessor
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
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Response(int code, string message = null, object data = null)
        {
            return new ResponseMessage(code, message, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Error(int code, string message = null, object data = null)
        {
            return Response(code, message, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IResponseMessage Success(object data)
        {
            return Response(0, data: data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(IGameContext context)
        {
            var (code, data) = await PreExecuteAsync(context);
            if (code != 0)
            {
                await context.Response.WriteAsync(code, null, data);
                return;
            }
            var result = await ExecuteCoreAsync(context);
            if (result == null)
            {
                await context.Response.WriteEmptyAsync();
            }
            else
            {
                await context.Response.WriteAsync(result);
            }
            await PostExecuteAsync(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task<(int code, object data)> PreExecuteAsync(IGameContext context)
        {
            return Task.FromResult<(int code,object data)>((0, null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<object> ExecuteCoreAsync(IGameContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual Task PostExecuteAsync(IGameContext context)
        {
            return Task.CompletedTask;
        }

       
    }
}
