using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IAvatar:IAvatarBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<string> GetAvatarNameAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask SaveAsync();
    }
}
