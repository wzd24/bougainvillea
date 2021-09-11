using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea.AdoNet;
using Scorpio.Bougainvillea.Props.Settings;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingsDefinitionProvider : IGameSettingDefinitionProvider
    {


        public void Define(IGameSettingDefinitionContext context) 
            => context.Add<GameSetting>(GameSettingScope.Global)
                      .Add<ConnectionStringSetting>(GameSettingScope.Global)
                      .Add<PropsSetting>(GameSettingScope.Global);
    }
}
