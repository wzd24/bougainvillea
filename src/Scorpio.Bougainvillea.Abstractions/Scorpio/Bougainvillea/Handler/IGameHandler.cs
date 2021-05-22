using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteAsync(IGameContext context);
    }
}
