using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Mails
{
    /// <summary>
    /// 官方邮件管理服务接口
    /// </summary>
    public interface IOfficialMailManager: IScopedDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="rewards"></param>
        /// <param name="excludeUsers"></param>
        /// <param name="sendTime"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task Add(int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serverId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="rewards"></param>
        /// <param name="excludeUsers"></param>
        /// <param name="sendTime"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task Update(int id,int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="serverId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="rewards"></param>
        /// <param name="excludeUsers"></param>
        /// <param name="sendTime"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task AddOrUpdate(int id, int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime);

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<int, MailExceptionState>> GetMailExceptions();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task SetMailExceptionState(int mailId, MailExceptionState state);
    }
}
