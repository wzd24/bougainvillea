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
    public interface IDepleteHandleManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depletion"></param>
        /// <param name="avatarId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        Task<object> Handle(int[] depletion,int avatarId,int serverId);
    }
}
