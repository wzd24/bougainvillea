using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public static class AvatarExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAvatar"></typeparam>
        /// <param name="currentUser"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static TAvatar GetCurrentAvatar<TAvatar>(this ICurrentUser currentUser,IGrainFactory  factory)
            where TAvatar:IAvatar
        {
           return factory.GetGrain<TAvatar>(currentUser.Id, currentUser.ServerId.ToString());
        }
    }
}
