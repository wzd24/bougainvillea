using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropsHandlerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propId"></param>
        /// <returns></returns>
        IPropsHandler GetHandler(int propId);
    }
}
