using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;
using Scorpio.Bougainvillea.Setting;
namespace Scorpio.Bougainvillea.Mails
{
    internal class MailSettingDefinitionProvider : ISettingDefinitionProvider
    {
        
        public void Define(ISettingDefinitionContext context)
        {
            context.Add(SettingDefinitionConsts.MailClientExpireHours, "客户端邮件超时小时数", defaultValue: 15 * 24);
            context.Add(SettingDefinitionConsts.MailServerExpireHours, "服务端邮件超时小时数", defaultValue: 15 * 24);
        }
    }
}
