using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Depletion;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Depletion
{
    internal class DepleteHandleManager : IDepleteHandleManager, IScopedDependency
    {

        private readonly IEnumerable<IDepleteHandlerProvider> _providers;
        private readonly IJsonSerializer _serializer;

        public DepleteHandleManager(IEnumerable<IDepleteHandlerProvider> providers, IJsonSerializer serializer)
        {
            _providers = providers;
            _serializer = serializer;
        }

        public ValueTask<int> CanHandleAsync(long[] depletion, int num)
        {
            var handler = _providers.Reverse().Select(provider => provider.GetHandler(depletion))
                .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的消耗处理器");
            var context = new DepleteHandleContext { Depletion = depletion, Num = num };
            return handler.CanExecuteAsync(context);
        }

        public async ValueTask<int> CanHandleAsync(string depletion, int num)
        {
            if (depletion.StartsWith("[["))
            {
                var rewardArray = _serializer.Deserialize<long[][]>(depletion);
                foreach (var item in rewardArray)
                {
                    var code = (await CanHandleAsync(item, num));
                    if (code != 0)
                    {
                        return code;
                    }
                }
                return (SystemErrorCodes.Success);
            }
            else
            {
                var reward = _serializer.Deserialize<long[]>(depletion);
                return await CanHandleAsync(reward, num);
            }
        }

        public ValueTask<int> HandleAsync(long[] depletion, int num, string reason)
        {
            var handler = _providers.Reverse().Select(provider => provider.GetHandler(depletion))
                .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的消耗处理器");
            var context = new DepleteHandleContext { Depletion = depletion, Reason = reason, Num = num };
            return handler.ExecuteAsync(context);
        }

        public async ValueTask<int> HandleAsync(string depletion, int num, string reson)
        {
            if (depletion.StartsWith("[["))
            {
                var rewardArray = _serializer.Deserialize<long[][]>(depletion);
                foreach (var item in rewardArray)
                {
                    var code = (await HandleAsync(item, num, reson));
                    if (code != 0)
                    {
                        return code;
                    }
                }
                return (SystemErrorCodes.Success);
            }
            else
            {
                var reward = _serializer.Deserialize<long[]>(depletion);
                return await HandleAsync(reward, num, reson);
            }
        }
    }
}
