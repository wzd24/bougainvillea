using System;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ApplicationServices { get; }

        /// <summary>
        /// 
        /// </summary>
        IGameRequest Request { get; }

        /// <summary>
        /// 
        /// </summary>
        IGameResponse Response { get; }

        /// <summary>
        /// 
        /// </summary>
        IGameUser User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IDictionary<object, object> Items { get; }

    }
}
