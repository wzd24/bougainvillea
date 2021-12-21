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
    public interface ICurrentServer
    {
        /// <summary>
        /// 
        /// </summary>
        int ServerId { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        IDisposable Use(int serverId);
    }
}
