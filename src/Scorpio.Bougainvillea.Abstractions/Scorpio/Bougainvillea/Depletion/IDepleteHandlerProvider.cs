using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Depletion
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDepleteHandlerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depletion"></param>
        /// <returns></returns>
        IDepleteHandler GetHandler(long[] depletion);
    }
}
