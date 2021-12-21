using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Rewards
{
    internal class RewardHandleManager : IRewardHandleManager, IScopedDependency
    {
        private readonly IEnumerable<IRewardHandlerProvider> _providers;
        private readonly IJsonSerializer _serializer;

        public RewardHandleManager(IEnumerable<IRewardHandlerProvider> providers,IJsonSerializer serializer)
        {
            _providers = providers;
            _serializer = serializer;
        }


        public async Task<(int code,object data)> HandleAsync(int[] reward, int num,string reason)
        {
            var handler = _providers.Reverse().Select(provider => provider.GetHandler(reward))
                .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的奖励处理器");
            var context = new RewardHandleContext { Rewards = reward,Num=num,Reason=reason};
            var result = await handler.ExecuteAsync(context);
            return result;
        }

        public async Task<(int code, object data)> HandleAsync(string rewards, int num, string reason)
        {
            if (rewards.StartsWith("[["))
            {
                var rewardArray = _serializer.Deserialize<int[][]>(rewards);
                var result = new List<(int code, object data)>();
                foreach (var item in rewardArray)
                {
                    result.Add(await HandleAsync(item, num, reason));
                }
                return (SystemErrorCodes.Success, result);
            }
            else
            {
                var reward = _serializer.Deserialize<int[]>(rewards);
                return await HandleAsync(reward, num, reason);
            }
        }
    }
}
