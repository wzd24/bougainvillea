using Scorpio.Bougainvillea.Middleware;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Handler
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate Task GameHandleDelegate(IGameContext context);
}
