using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Props.Settings;
using Scorpio.Bougainvillea.Rewards;
using Scorpio.Bougainvillea.Setting;

namespace Scorpio.Bougainvillea.Props
{
    internal class RewardPropsHandler : IPropsHandler
    {
        private readonly IRewardHandleManager _rewardHandleManager;
        private readonly IGameSettingManager _gameSettingManager;
        private readonly IJsonSerializer _jsonSerializer;

        public RewardPropsHandler(IRewardHandleManager rewardHandleManager, IGameSettingManager gameSettingManager, IJsonSerializer jsonSerializer)
        {
            _rewardHandleManager = rewardHandleManager;
            _gameSettingManager = gameSettingManager;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<(int code, object data)> UseAsync(PropsHandleContext context)
        {
            var setting = (await _gameSettingManager.GetAsync<PropsSetting>()).GetOrDefault(context.PropId);
            if (setting == null)
            {
                return (PropsErrorCodes.NotExist, null);
            }
            return await _rewardHandleManager.Handle(_jsonSerializer.Deserialize<int[]>(setting.Effect), context.Num);
        }
    }
}
