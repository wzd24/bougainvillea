using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea;

namespace Sailina.Tang.Essential.BeautySystem
{
    internal class BeautySystemDefinitionProvider : ISettingDefinitionProvider
    {

        public void Define(ISettingDefinitionContext context)
        {
            context.Add(SettingDefinitionConsts.BeautyGiftListSettingDefinitionName, "情缘赠送道具列表", defaultValue: "[30005,30004,30003,30006,30007]");
            //context.Add(SettingDefinitionConsts.MailServerExpireHours, "服务端邮件超时小时数", defaultValue: 15 * 24);
        }
    }
}
