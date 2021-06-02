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
    public interface IUserMailManager:ITransientDependency
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> BuildOfficialMailsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="state"></param>
        /// <param name="rewards"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task SendMailAsync(UserMailType type, string title, string content, MailExceptionState state, string rewards, DateTime expireTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        Task<int> CanReadMailAsync(int mailId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        Task<(int code, object data)> ReadMailAsync(int mailId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        Task DeleteMailAsync(int mailId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailType"></param>
        /// <returns></returns>
        Task<(int code, object data)> ReadAllMailsAsync(UserMailType mailType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailType"></param>
        /// <returns></returns>
        Task DeleteReadedMailsAsync(UserMailType mailType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task UpdateOfficialMailExceptionAsync();
    }
}
