using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingDefinitionContext : IGameSettingDefinitionContext
    {
        private readonly Dictionary<string, GameSettingDefinition> _settingDefinitions;

        public GameSettingDefinitionContext(Dictionary<string, GameSettingDefinition> settingDefinitions) => _settingDefinitions = settingDefinitions;

        public void Add(params GameSettingDefinition[] settingDefinitions)
        {
            if (settingDefinitions.IsNullOrEmpty())
            {
                return;
            }
            foreach (var definition in settingDefinitions)
            {
                _settingDefinitions.AddOrUpdate(definition.Name,_=>definition);
            }
        }
        public virtual IReadOnlyList<GameSettingDefinition> GetAll() => _settingDefinitions.Values.ToImmutableList();

        public GameSettingDefinition GetOrNull(string name) => _settingDefinitions.GetOrDefault(name);

        public GameSettingDefinition GetOrNull<T>() => GetOrNull(typeof(T).Name);
    }
}
