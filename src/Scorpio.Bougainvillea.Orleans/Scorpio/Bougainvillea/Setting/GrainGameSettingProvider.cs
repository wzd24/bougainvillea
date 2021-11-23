using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using Orleans;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    internal abstract class GrainGameSettingProvider : IGameSettingProvider, ISingletonDependency
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ICurrentServer _currentServer;
        private readonly IMemoryCache _memoryCache;
        private readonly MethodInfo _getValue = typeof(GrainGameSettingProvider).GetMethod(nameof(GetValueAsync), BindingFlags.NonPublic | BindingFlags.Instance);
        protected GrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer, IMemoryCache memoryCache)
        {
            _grainFactory = grainFactory;
            _currentServer = currentServer;
            _memoryCache = memoryCache;
        }
        public abstract GameSettingScope Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        public async Task<GameSettingValue> GetAsync(GameSettingDefinition settingDefinition)
        {
            if (settingDefinition.Scope == GameSettingScope.Server && _currentServer.ServerId == 0)
            {
                return null;
            }
            var value = _memoryCache.Get<GameSettingValue>(settingDefinition.Name);
            if (value == null)
            {
                var method = _getValue.MakeGenericMethod(settingDefinition.ValueType);
                var task = (ValueTask<GameSettingValue>)method.Invoke(this, new object[] { _currentServer.ServerId, settingDefinition });
                value = await task;
                _memoryCache.Set(settingDefinition.Name, value,DateTimeOffset.Now.AddMinutes(30).AddSeconds(Random.Shared.Next(0, 60)));
            }
            return value;
        }

        private async ValueTask<GameSettingValue> GetValueAsync<T>(int serverId, GameSettingDefinition gameSettingDefinition)
            where T : GameSettingBase
        {
            if (serverId != 0)
            {
                var grain = _grainFactory.GetGrain<IServerSettingManager>(_currentServer.ServerId);
                return new GameSettingValue<T>(await grain.GetAsync<T>()) { Definition = gameSettingDefinition };
            }
            else
            {
                var grain = _grainFactory.GetGrain<IGlobalSettingManager>(Guid.Empty);
                return new GameSettingValue<T>(await grain.GetAsync<T>()) { Definition = gameSettingDefinition };
            }
        }

        public Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, T value) where T : GameSettingBase => throw new NotImplementedException();
        public Task SetAsync<T>(GameSettingDefinition<T> settingDefinition, IReadOnlyCollection<T> values) where T : GameSettingBase => throw new NotImplementedException();
    }

    internal class GlobalGrainGameSettingProvider : GrainGameSettingProvider
    {
        public GlobalGrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer, IMemoryCache memoryCache) : base(grainFactory, currentServer, memoryCache)
        {
        }

        public override GameSettingScope Scope => GameSettingScope.Global;
    }

    internal class ServerGrainGameSettingProvider : GrainGameSettingProvider
    {
        public ServerGrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer, IMemoryCache memoryCache) : base(grainFactory, currentServer, memoryCache)
        {
        }

        public override GameSettingScope Scope => GameSettingScope.Server;
    }
}
