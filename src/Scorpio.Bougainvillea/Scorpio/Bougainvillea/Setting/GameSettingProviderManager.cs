using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Options;

using Orleans.Runtime;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingProviderManager : IGameSettingProviderManager, ISingletonDependency
    {
        private readonly Lazy<List<IGameSettingProvider>> _providers;
        private readonly IServiceProvider _serviceProvider;
        private readonly GameSettingOptions _options;

        public ICollection<IGameSettingProvider> Providers => _providers.Value;

        public GameSettingProviderManager(IServiceProvider serviceProvider,
            IOptions<GameSettingOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
            _providers = new Lazy<List<IGameSettingProvider>>(() =>
                _options.SettingProviders.Select(t => _serviceProvider.GetService(t) as IGameSettingProvider).ToList(), true);
        }
    }
}
