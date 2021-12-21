using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Essential
{
    internal class AvatarSettingDefinitionProvider : ISettingDefinitionProvider
    {
        public void Define(ISettingDefinitionContext context)
        {
            context.Add(AvatarSettingDefinitionConsts.NickNameMinMaxLength, "玩家昵称最小最大长度配置", defaultValue: new List<int> { 4, 12 });
        }
    }
}
