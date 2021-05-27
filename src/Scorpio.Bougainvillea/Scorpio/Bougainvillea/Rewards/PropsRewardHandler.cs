using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Props;

namespace Scorpio.Bougainvillea.Rewards
{
    [RewardHandler(2)]
    internal class PropsRewardHandler : IRewardHandler
    {
        private readonly IPropsHandleManager _propsHandleManager;

        public PropsRewardHandler(IPropsHandleManager propsHandleManager)
        {
            _propsHandleManager = propsHandleManager;
        }
        public async Task<(int code, object data)> ExecuteAsync(RewardHandleContext context)
        {
            if (context.Rewards.Length != 3)
            {
                return (-1, null);
            }
            var propsId = context.Rewards[1];
            var num = context.Rewards[2];
            return await _propsHandleManager.AddPropAsync(propsId, num * context.Num, context.Reason);
        }
    }
}
