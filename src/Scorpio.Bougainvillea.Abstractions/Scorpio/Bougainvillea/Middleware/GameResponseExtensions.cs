using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameResponseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task WriteEmptyAsync(this IGameResponse response)
        {
            var resp = new ResponseMessage
            {
                Code = 0,
                Message = "Successed"
            };

            await WriteAsync(response, resp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this IGameResponse response, object content)
        {
            switch (content)
            {
                case IResponseMessage message:
                    await response.WriteAsync(message);
                    break;
                case string value:
                    await response.WriteStringAsync(value);
                    break;
                default:
                    var resp = new ResponseMessage
                    {
                        Code = 0,
                        Message = "Successed",
                        Data = content
                    };

                    await WriteAsync(response, resp);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteStringAsync(this IGameResponse response, string content)
        {
            var resp = new ResponseMessage
            {
                Code = 0,
                Message = "Successed",
                Data = content
            };

            await WriteAsync(response, resp);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this IGameResponse response, int code, string message, object content)
        {
            var resp = new ResponseMessage
            {
                Code = code,
                Message = message,
                Data = content
            };

            await WriteAsync(response, resp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task WriteAsync(this IGameResponse response, IResponseMessage message)
        {
            await response.WriteAsync(response.Context.ApplicationServices.GetService<IJsonSerializer>().Serialize(message));
        }


        class ResponseMessage : IResponseMessage
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }
}
