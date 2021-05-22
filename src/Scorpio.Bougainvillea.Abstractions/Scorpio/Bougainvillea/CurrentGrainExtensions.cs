using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public static class CurrentGrainExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TGrain"></typeparam>
        /// <param name="currentUser"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static TGrain GetCurrentUserGrain<TGrain>(this ICurrentUser currentUser, IGrainFactory factory)
            where TGrain:IGrainWithIntegerCompoundKey
        {
            return factory.GetGrain<TGrain>(currentUser.Id, currentUser.ServerId.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TGrain"></typeparam>
        /// <param name="currentUser"></param>
        /// <param name="factory"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TGrain GetCurrentUserGrain<TGrain>(this ICurrentUser currentUser, IGrainFactory factory,int id)
            where TGrain : IGrainWithIntegerCompoundKey
        {
            return factory.GetGrain<TGrain>(id, $"{currentUser.Id}-{currentUser.ServerId}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TGrain"></typeparam>
        /// <param name="currentServer"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static TGrain GetCurrentServerGrain<TGrain>(this ICurrentServer  currentServer, IGrainFactory factory)
            where TGrain : IGrainWithIntegerKey
        {
            return factory.GetGrain<TGrain>(currentServer.ServerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TGrain"></typeparam>
        /// <param name="currentServer"></param>
        /// <param name="factory"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TGrain GetCurrentServerGrain<TGrain>(this ICurrentServer currentServer, IGrainFactory factory, int id)
            where TGrain : IGrainWithIntegerCompoundKey
        {
            return factory.GetGrain<TGrain>(id, currentServer.ServerId.ToString());
        }

    }
}
