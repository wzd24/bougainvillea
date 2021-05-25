using System.Collections.Generic;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingStoreContext:IGameSettingStoreContext
    {

        public GameSettingStoreContext(GameSettingDefinition settingDefinition)
        {
            SettingDefinition = settingDefinition;
            Properties = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Properties { get; }
        public GameSettingDefinition SettingDefinition { get; }
        public int Key { get; set; }
    }
}