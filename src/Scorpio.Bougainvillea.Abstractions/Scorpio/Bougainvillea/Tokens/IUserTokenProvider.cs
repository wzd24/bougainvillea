using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Tokens
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserTokenProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateToken(UserData user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserData GetUserData(string token);
    }
}
