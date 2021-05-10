using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameHandlerFactory : IServiceProviderAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        GameHandleDelegate Create(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="handler"></param>
        void AddHandler(string code, GameHandleDelegate handler);
    }
}
