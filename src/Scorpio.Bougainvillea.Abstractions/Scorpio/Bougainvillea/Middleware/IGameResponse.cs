using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameResponse
    {

        /// <summary>
        /// 
        /// </summary>
        IDictionary<string,string> Headers { get; }

        /// <summary>
        /// 
        /// </summary>
        IGameContext Context { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        Task WriteAsync(string value);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task FlushAsync();

    }
}
