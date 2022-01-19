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
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Setting
{
    internal abstract class GrainGameSettingProvider : IGameSettingProvider, ISingletonDependency
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ICurrentServer _currentServer;
        protected GrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer)
        {
            _grainFactory = grainFactory;
            _currentServer = currentServer;
        }
        public abstract GameSettingScope Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        public async ValueTask<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition settingDefinition) where T : GameSettingBase
        {
            if (settingDefinition.Scope == GameSettingScope.Server && _currentServer.ServerId == 0)
            {
                return null;
            }
            return await GetValueAsync<T>(_currentServer.ServerId, settingDefinition);
        }

        public async ValueTask<GameSettingValue<T>> GetAsync<T>(GameSettingDefinition settingDefinition, int id) where T : GameSettingBase
        {
            if (settingDefinition.Scope == GameSettingScope.Server && _currentServer.ServerId == 0)
            {
                return null;
            }
            return await GetValueAsync<T>(_currentServer.ServerId, settingDefinition, id);
        }

        public async ValueTask<int> GetMaxIdAsync<T>(GameSettingDefinition settingDefinition) where T : GameSettingBase
        {
            if (settingDefinition.Scope == GameSettingScope.Server && _currentServer.ServerId == 0)
            {
                return 0;
            }
            return await GetMaxIdAsync<T>(_currentServer.ServerId, settingDefinition);
        }

        private async ValueTask<GameSettingValue<T>> GetValueAsync<T>(int serverId, GameSettingDefinition gameSettingDefinition)
            where T : GameSettingBase
        {
            if (serverId != 0)
            {
                var grain = _grainFactory.GetGrain<IServerSettingManager>(_currentServer.ServerId, gameSettingDefinition.Name);
                return new GameSettingValue<T>(await grain.GetAsync<T>()) { Definition = gameSettingDefinition };
            }
            else
            {
                var grain = _grainFactory.GetGrain<IGlobalSettingManager>(gameSettingDefinition.Name);
                return new GameSettingValue<T>(await grain.GetAsync<T>()) { Definition = gameSettingDefinition };
            }
        }

        private async ValueTask<int> GetMaxIdAsync<T>(int serverId, GameSettingDefinition gameSettingDefinition)
    where T : GameSettingBase
        {
            if (serverId != 0)
            {
                var grain = _grainFactory.GetGrain<IServerSettingManager>(_currentServer.ServerId, gameSettingDefinition.Name);
                return await grain.GetMaxIdAsync<T>();
            }
            else
            {
                var grain = _grainFactory.GetGrain<IGlobalSettingManager>(gameSettingDefinition.Name);
                return await grain.GetMaxIdAsync<T>();
            }
        }

        private async ValueTask<GameSettingValue<T>> GetValueAsync<T>(int serverId, GameSettingDefinition gameSettingDefinition, int id)
    where T : GameSettingBase
        {
            if (serverId != 0)
            {
                var grain = _grainFactory.GetGrain<IServerSettingManager>(_currentServer.ServerId, gameSettingDefinition.Name);
                return new GameSettingValue<T>(new List<T> { await grain.GetAsync<T>(id) }) { Definition = gameSettingDefinition };
            }
            else
            {
                var grain = _grainFactory.GetGrain<IGlobalSettingManager>(gameSettingDefinition.Name);
                return new GameSettingValue<T>(new List<T> { await grain.GetAsync<T>(id) }) { Definition = gameSettingDefinition };
            }
        }

    }

    internal class GlobalGrainGameSettingProvider : GrainGameSettingProvider
    {
        public GlobalGrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer) : base(grainFactory, currentServer)
        {
        }

        public override GameSettingScope Scope => GameSettingScope.Global;
    }

    internal class ServerGrainGameSettingProvider : GrainGameSettingProvider
    {
        public ServerGrainGameSettingProvider(IGrainFactory grainFactory, ICurrentServer currentServer) : base(grainFactory, currentServer)
        {
        }

        public override GameSettingScope Scope => GameSettingScope.Server;
    }
}
