using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Sailina.Tang.Essential.Settings
{
    internal class GameSettingsDefinitionProvider : IGameSettingDefinitionProvider
    {


        public void Define(IGameSettingDefinitionContext context)
            => context.Add<HeroSetting>(GameSettingScope.Global)
                      .Add<HeroLevelSetting>(GameSettingScope.Global)
                      .Add<HeroStudySetting>(GameSettingScope.Global);
    }
}
