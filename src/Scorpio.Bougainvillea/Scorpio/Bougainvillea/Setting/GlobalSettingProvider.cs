using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{

    internal class GlobalSettingProvider : SettingProvider,ISingletonDependency
    {
        public GlobalSettingProvider(ISettingStore settingStore) : base(settingStore)
        {
        }

        public override string Name => "Global";
        protected override string Key => "0";
    }
}
