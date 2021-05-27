using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Depletion;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Depletion
{
    internal class DepleteHandleManager : IDepleteHandleManager, ISingletonDependency
    {
        private readonly IEnumerable<IDepleteHandlerProvider> _providers;

        public DepleteHandleManager(IEnumerable<IDepleteHandlerProvider> providers)
        {
            _providers = providers;
        }


        public async Task<(int code, object data)> Handle(int[] depletion, int num, string reason)
        {
            var handler = _providers.Reverse().Select(provider => provider.GetHandler(depletion))
                .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的奖励处理器");
            var context = new DepleteHandleContext { Depletion = depletion, Reason = reason, Num = num };
            var result = await handler.ExecuteAsync(context);
            return result;
        }
    }
}
