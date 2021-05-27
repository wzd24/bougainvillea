using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Props;

namespace Scorpio.Bougainvillea.Depletion
{
    [DepleteHandler(2)]
    internal class PropsDepleteHandler : IDepleteHandler
    {
        private readonly IPropsHandleManager _propsHandleManager;

        public PropsDepleteHandler(IPropsHandleManager propsHandleManager)
        {
            _propsHandleManager = propsHandleManager;
        }
        public async Task<(int code, object data)> ExecuteAsync(DepleteHandleContext context)
        {
            if (context.Depletion.Length != 3)
            {
                return (-1, null);
            }
            var propsId = context.Depletion[1];
            var num = context.Depletion[2];
            return await _propsHandleManager.ConsumeAsync(propsId, num * context.Num, context.Reason);
        }
    }
}
