using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICurrentUser
    {

        /// <summary>
        /// 
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// 
        /// </summary>
        int AvatarId { get; }

        /// <summary>
        /// 
        /// </summary>
        int ServerId { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsAuthentication { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IDisposable Use(IGameUser user);
    }
}