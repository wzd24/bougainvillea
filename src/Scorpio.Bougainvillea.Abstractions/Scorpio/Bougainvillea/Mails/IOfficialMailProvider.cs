using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Mails
{

    /// <summary>
    /// 
    /// </summary>
    public interface IOfficialMailProvider:ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OfficialMail> GetMailAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OfficialMail>> GetMailsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<int, int>> GetMailExceptionsAsync();
    }
}
