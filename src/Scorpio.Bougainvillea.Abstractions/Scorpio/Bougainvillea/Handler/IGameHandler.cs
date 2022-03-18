using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameHandler : ISingletonDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteAsync(IGameContext context);
    }
}
