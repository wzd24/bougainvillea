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
    public class UserMail
    {
        /// <summary>
        /// 邮件Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 邮件编号
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 邮件区服Id
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 邮件奖励内容
        /// </summary>
        public string Rewards { get; set; }

        /// <summary>
        ///  读取状态：false 未读；true 已读
        /// </summary>
        public bool ReadState { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public UserMailType Type { get; set; }

        /// <summary>
        /// 邮件异常状态
        /// </summary>
        public MailExceptionState ExceptionState { get; set; }

        /// <summary>
        /// 邮件创建时间
        /// </summary>
        public DateTime BuildTime { get; set; }

        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 邮件过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; } = DateTime.MaxValue;
        /// <summary>
        /// 客户端邮件过期时间
        /// </summary>
        public DateTime ClientExpireTime { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        public UserMail()
        {
            ReadTime = default;
            ReadState = false;
            BuildTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 用户邮件类别
    /// </summary>
    public enum UserMailType
    {
        /// <summary>
        /// 系统邮件
        /// </summary>
        System,

        /// <summary>
        /// 个人邮件
        /// </summary>
        User
    }
}
