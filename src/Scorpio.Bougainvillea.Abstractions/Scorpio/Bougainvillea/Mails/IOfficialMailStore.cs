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
    public interface IOfficialMailStore:ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        Task Save(OfficialMail mail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OfficialMail> GetMail(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OfficialMail>> GetMails();
    }
}
