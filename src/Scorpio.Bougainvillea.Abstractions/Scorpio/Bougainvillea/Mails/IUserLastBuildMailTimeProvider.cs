using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Mails
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserLastBuildMailTimeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<DateTime> GetLastBuildMailTimeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task SetLastBuildMailTimeAsync(DateTime dateTime);
    }
}
