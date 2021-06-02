using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    internal class GameSettingSettingStore : ISettingStore, ISingletonDependency
    {
        private readonly IGameSettingManager _gameSettingManager;
        private readonly IJsonSerializer _jsonSerializer;

        public GameSettingSettingStore(IGameSettingManager gameSettingManager,IJsonSerializer jsonSerializer)
        {
            _gameSettingManager = gameSettingManager;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<SettingValue<T>> GetAsync<T>(ISettingStoreContext context)
        {
            var settings =await _gameSettingManager.GetAsync<GameSetting>();
            var key = int.Parse(context.Properties["ProviderKey"].ToString());
            var name = context.SettingDefinition.Name;
            var result = settings.Values.FirstOrDefault(s => s.ServerId == key && s.Name == name);
            return new SettingValue<T> { Definition = context.SettingDefinition, Value = _jsonSerializer.Deserialize<T>(result.Value) };
        }

        public Task SetAsync<T>(ISettingStoreContext context, T value) => throw new NotImplementedException();
    }
}
