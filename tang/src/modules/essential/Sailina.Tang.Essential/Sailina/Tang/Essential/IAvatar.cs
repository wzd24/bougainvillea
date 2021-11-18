using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAvatar:IAvatarBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<string> GetAvatarNameAsync();
    }
}
