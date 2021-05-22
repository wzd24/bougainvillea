using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GlobalGameSettingProvider : GameSettingProvider
    {
        public GlobalGameSettingProvider(IGameSettingStore settingStore) : base(settingStore)
        {
        }

        public override GameSettingScope Scope { get; } = GameSettingScope.Global;
    }
}
