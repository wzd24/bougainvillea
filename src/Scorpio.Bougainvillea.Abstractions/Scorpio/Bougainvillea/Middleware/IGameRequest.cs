using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameRequest
    {
        /// <summary>
        /// 
        /// </summary>
        IReadOnlyDictionary<string, string> Headers { get; }

        /// <summary>
        /// 
        /// </summary>
        string RequestCode { get; }

        /// <summary>
        /// 
        /// </summary>
        string Content { get; set; }
    }
}
