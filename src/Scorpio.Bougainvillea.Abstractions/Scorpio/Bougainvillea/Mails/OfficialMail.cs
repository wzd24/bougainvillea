using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Mails
{
    /// <summary>
    /// 官方邮件
    /// </summary>
    public class OfficialMail
    {

        /// <summary>
        /// 邮件Id
        /// </summary>
        public int Id { get; set; }

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
        /// 发送排除玩家列表
        /// </summary>
        public long[] ExcludeUsers { get; set; }


        /// <summary>
        /// 邮件异常状态
        /// </summary>
        public MailExceptionState ExceptionState { get; set; }

        /// <summary>
        /// 邮件发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 邮件最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 邮件过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; } = DateTime.MaxValue;

    }

    /// <summary>
    /// 邮件异常状态
    /// </summary>
    public enum MailExceptionState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal=0,

        /// <summary>
        /// 屏蔽
        /// </summary>
        Forbid=1,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted=2,

        /// <summary>
        /// 已修改
        /// </summary>
        Modified=3,
    }
}