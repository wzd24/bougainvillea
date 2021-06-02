using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public static class CurrentUserExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="serverId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public static IDisposable Use(this ICurrentUser currentUser, int serverId,int avatarId)
        {
            return (currentUser as CurrentUser).Use(serverId, avatarId);
        }
    }
}
