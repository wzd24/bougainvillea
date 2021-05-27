using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Rewards
{
    internal class RewardHandleManager : IRewardHandleManager,ISingletonDependency
    {
        private readonly IEnumerable<IRewardHandlerProvider> _providers;

        public RewardHandleManager(IEnumerable<IRewardHandlerProvider> providers)
        {
            _providers = providers;
        }


        public async Task<(int code,object data)> Handle(int[] reward, int num,string reason)
        {
            var handler = _providers.Reverse().Select(provider => provider.GetHandler(reward))
                .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的奖励处理器");
            var context = new RewardHandleContext { Rewards = reward,Num=num,Reason=reason};
            var result = await handler.ExecuteAsync(context);
            return result;
        }
    }
}
