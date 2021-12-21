using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Scorpio;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingDefinitionManager : IGameSettingDefinitionManager, ISingletonDependency
    {
        private readonly Lazy<IDictionary<string, GameSettingDefinition>> _settingDefinitions;
        private readonly IServiceProvider _serviceProvider;
        private readonly GameSettingOptions _options;

        public GameSettingDefinitionManager(IOptions<GameSettingOptions> options, IServiceProvider serviceProvider)
        {
            _settingDefinitions = new Lazy<IDictionary<string, GameSettingDefinition>>(CreateSettingDefinitions, true);
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public GameSettingDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var setting = _settingDefinitions.Value.GetOrDefault(name);
            if (setting == null)
            {
                throw new ScorpioException("Undefined setting: " + name);
            }
            return setting;
        }

        public GameSettingDefinition<T> Get<T>() where T : GameSettingBase
        {
            return Get(typeof(T).Name) as GameSettingDefinition<T>;
        }

        public virtual IReadOnlyList<GameSettingDefinition> GetAll() => _settingDefinitions.Value.Values.ToImmutableList();

        protected virtual IDictionary<string, GameSettingDefinition> CreateSettingDefinitions()
        {
            var settings = new Dictionary<string, GameSettingDefinition>();
            using (var scope = _serviceProvider.CreateScope())
            {
                var providers = _options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as IGameSettingDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(new GameSettingDefinitionContext(settings));
                }
            }
            return settings;
        }
    }
}
