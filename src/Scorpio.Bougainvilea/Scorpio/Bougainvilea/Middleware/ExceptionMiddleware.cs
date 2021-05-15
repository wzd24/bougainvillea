using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Scorpio.Middleware.Pipeline;

namespace Scorpio.Bougainvillea.Middleware
{
    class ExceptionMiddleware
    {
        private readonly PipelineRequestDelegate<IGameContext> _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(PipelineRequestDelegate<IGameContext> next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(IGameContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                await ExpandExceptionAsync(context, ex);
            }
        }

        private static async Task ExpandExceptionAsync(IGameContext context, Exception ex)
        {
            if (ex.InnerException != null && !(ex is GameFriendlyException))
            {
                await ExpandExceptionAsync(context, ex.InnerException);
            }
            else
            {
                await context.Response.ClearAsync();
                var error = ex switch
                {
                    GameFriendlyException s => new Error(s.Code, s.Message, s.Data),
                    _ => new Error(StatusCodes.Status500InternalServerError, ex.Message.ToString(), null)
                };
                await context.Response.WriteAsync(error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Error : IResponseMessage
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="message"></param>
            /// <param name="code"></param>
            /// <param name="data"></param>
            public Error(int code, string message, object data)
            {
                Message = message;
                Code = code;
                Data = data;
            }

            /// <summary>
            /// 
            /// </summary>
            public string Message { get; }

            /// <summary>
            /// 
            /// </summary>
            public int Code { get; }

            /// <summary>
            /// 
            /// </summary>
            public object Data { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Message;
            }
        }

    }
}
