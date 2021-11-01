using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Scorpio.DependencyInjection;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingSettingStore : ISettingStore, ISingletonDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private readonly IJsonSerializer _jsonSerializer;

        public GameSettingSettingStore(IGameSettingManager gameSettingManager, IJsonSerializer jsonSerializer)
        {
            _gameSettingManager = gameSettingManager;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<SettingValue<T>> GetAsync<T>(ISettingStoreContext context)
        {
            var settings = await _gameSettingManager.GetAsync<GameSetting>();
            var key = int.Parse(context.Properties["ProviderKey"].ToString());
            var name = context.SettingDefinition.Name;
            var result = settings.FirstOrDefault(s => s.ServerId == key && s.Name == name);
            return new SettingValue<T> { Definition = context.SettingDefinition, Value = _jsonSerializer.Deserialize<T>(result.Value) };
        }

        public async Task SetAsync<T>(ISettingStoreContext context, T value)
        {
            var settings = await _gameSettingManager.GetAsync<GameSetting>();
            var key = int.Parse(context.Properties["ProviderKey"].ToString());
            var name = context.SettingDefinition.Name;
            var result = settings.FirstOrDefault(s => s.ServerId == key && s.Name == name);
            if (result == null)
            {
                result = new GameSetting { ServerId = key, Name = name, DisplayName = context.SettingDefinition.DisplayName };
            }
            result.Value = _jsonSerializer.Serialize(value);
            await _gameSettingManager.SetAsync(result);
        }
    }
}
