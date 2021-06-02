using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ServerSettingProvider : SettingProvider, ISingletonDependency
    {
        private readonly ICurrentServer _currentServer;

        public ServerSettingProvider(ISettingStore settingStore,ICurrentServer currentServer) : base(settingStore)
        {
            _currentServer = currentServer;
        }

        public override string Name => "Server";
        protected override string Key => _currentServer.ServerId.ToString();
    }
}
