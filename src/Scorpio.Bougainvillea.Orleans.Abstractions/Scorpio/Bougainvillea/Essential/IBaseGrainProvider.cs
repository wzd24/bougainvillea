using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseGrainProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IGameBase   GetGameBase();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        IServerBase GetServerBase(int serverId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        IAvatarBase GetAvatarBase(long avatarId);
    }
}
