using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Orleans.Streams;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public static class GrainExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grain"></param>
        /// <param name="streamId"></param>
        /// <param name="streamNamesapce"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IAsyncStream<T> GetStreamAsync<T>(this GrainBase grain, Guid streamId, string streamNamesapce, string providerName = null)
        {
            var provider = grain.GetStreamProvider(providerName);
            return provider.GetStream<T>(streamId, streamNamesapce);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grain"></param>
        /// <param name="key"></param>
        /// <param name="streamNamesapce"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IAsyncStream<T> GetStreamAsync<T>(this GrainBase grain, long key, string streamNamesapce, string providerName = null)
        {
            return GetStreamAsync<T>(grain, 0, key, streamNamesapce, providerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="grain"></param>
        /// <param name="n0Key"></param>
        /// <param name="n1Key"></param>
        /// <param name="streamNamesapce"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IAsyncStream<T> GetStreamAsync<T>(this GrainBase grain, long n0Key, long n1Key, string streamNamesapce, string providerName = null)
        {
            return GetStreamAsync<T>(grain, ToGuidKey(n0Key, n1Key), streamNamesapce, providerName);
        }

        private static Guid ToGuidKey(long n0Key, long n1Key)
        {
            return new Guid((uint)(n0Key & 0xffffffff), (ushort)(n0Key >> 32), (ushort)(n0Key >> 48), (byte)n1Key, (byte)(n1Key >> 8), (byte)(n1Key >> 16), (byte)(n1Key >> 24), (byte)(n1Key >> 32), (byte)(n1Key >> 40), (byte)(n1Key >> 48), (byte)(n1Key >> 56));
        }

        private static long[] ToLongKey(Guid key)
        {
            var guidKeyBytes = key.ToByteArray();
            return new long[] { BitConverter.ToInt64(guidKeyBytes, 0), BitConverter.ToInt64(guidKeyBytes, 8) };
        }

    }
}
