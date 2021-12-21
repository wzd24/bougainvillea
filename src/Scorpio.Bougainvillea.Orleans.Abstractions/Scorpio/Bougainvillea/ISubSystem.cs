using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISubSystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarBase"></param>
        /// <returns></returns>
        ValueTask OnSetupAsync(IAvatarBase avatarBase);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask InitializeAsync();
    }
}
