using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Setting;
using Scorpio.Bougainvillea.Setting;
using Scorpio.Bougainvillea.AdoNet;
using Scorpio.Bougainvillea.Props.Settings;
using Scorpio.Bougainvillea.Essential;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingsDefinitionProvider : IGameSettingDefinitionProvider
    {


        public void Define(IGameSettingDefinitionContext context)
            => context.Add<GameSetting>(GameSettingScope.Global)
                      .Add<ConnectionStringSetting>(GameSettingScope.Global)
                      .Add<PropsSetting>(GameSettingScope.Global)
                      .Add<RoleSetting>(GameSettingScope.Global, new List<RoleSetting> {
                          new RoleSetting{
                              HeadFrameId = 62001,
                              Heads=new List<int> {61001,61002,61003,61004,61005,61006},
                              Id=1,
                              Image=63001,
                              Sex= Sex.Male
                          },
                          new RoleSetting{
                              HeadFrameId = 62001,
                              Heads=new List<int> {61007,61008,61009,61010,61011,61012},
                              Id=1,
                              Image=63101,
                              Sex= Sex.Male
                          }
                      });
    }
}
