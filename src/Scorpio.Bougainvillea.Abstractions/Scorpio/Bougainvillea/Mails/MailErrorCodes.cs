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
    public static class MailErrorCodes
    {
        /// <summary>
        /// 基础错误代码
        /// </summary>
        public static int BaseCode { get; set; } = 100;

        /// <summary>
        /// 邮件不存在
        /// </summary>
        public static int NotExists => BaseCode + 1;
        /// <summary>
        /// 邮件不存在
        /// </summary>
        public static int MailReaded => BaseCode + 2;

        /// <summary>
        /// 邮件已过期
        /// </summary>
        public static int Expired => BaseCode + 3;


    }
}
