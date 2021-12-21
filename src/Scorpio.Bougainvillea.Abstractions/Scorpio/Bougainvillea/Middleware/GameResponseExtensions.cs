using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Handler;

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
            var resp = new ResponseMessage(0, "Successed");

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
                default:
                    var resp = new ResponseMessage(0,"Successed",content);

                    await WriteAsync(response, resp);
                    break;
            }
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
            var resp = new ResponseMessage(code, message, content);
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
            await response.WriteAsync(response.Context.ApplicationServices.GetService<IResponseSerializer>().Serialize(message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsSuccessed(this IResponseMessage message)
        {
            if (message is ISuccessd successd)
            {
                return successd.IsSuccessed();
            }
            return message.Code == 0;
        }
    }
}
