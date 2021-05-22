using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.Handler;

namespace Scorpio.Bougainvillea.Props
{
    internal class PropsHandleManager : IPropsHandleManager
    {
        private readonly Lazy<IEnumerable<IPropsHandlerProvider>> _providers;
        private readonly PropsHandleOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public IEnumerable<IPropsHandlerProvider> Providers => _providers.Value;

        public PropsHandleManager(IServiceProvider serviceProvider,IOptions<PropsHandleOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _providers = new Lazy<IEnumerable<IPropsHandlerProvider>>(() =>
                  _options.HandlerProviders.Select(t => _serviceProvider.GetService(t) as IPropsHandlerProvider), true);
        }

        public Task<(int code, object data)> AddPropAsync(int propId, long num)
        {
            return Handle(propId, num, null, (h, c) => h.AddPropAsync(c));
        }

        public async Task<int> CanUseAsync(int propId, long num, dynamic para = null)
        {
            return (await Handle(propId, num, null, async (h, c) => (await h.CanUseAsync(c), null))).code;
        }

        public async Task<(int code, object data)> ConsumeAsync(int propId, long num)
        {
            var code = await EnoughAsync(propId, num);
            if (code != 0)
            {
                return (code, null);
            }
            return await Handle(propId, num, null, (h, c) => h.ConsumeAsync(c));
        }

        public async Task<(int,Dictionary<int, (int code, object data)>)> ConsumeAsync(Dictionary<int, long> props)
        {
            var (code, r) = await EnoughAsync(props);
            if (code != 0)
            {
                return (code, r.ToDictionary(kv => kv.Key, kv => ( kv.Value,default(object))));
            }
            var result = new Dictionary<int, (int code, object data)>();
            await props.ForEachAsync(async kv => result.Add(kv.Key, await ConsumeAsync(kv.Key, kv.Value)));
            return (result.Select(kv => kv.Value.code).FirstOrDefault(v => v != 0), result);
        }

        public async Task<int> EnoughAsync(int propId, long num)
        {
            return (await Handle(propId, num, null, async (h, c) => (await h.EnoughAsync(c), null))).code;
        }

        public async Task<(int, Dictionary<int, int>)> EnoughAsync(Dictionary<int, long> props)
        {
            var result = new Dictionary<int, int>();
            await props.ForEachAsync(async kv => result.Add(kv.Key, await EnoughAsync(kv.Key, kv.Value)));
            return (result.Select(kv => kv.Value).FirstOrDefault(v => v != 0), result);
        }

        public Task<(int code, object data)> UseAsync(int propId, int num, dynamic para = null)
        {
            Func<IPropsHandler, PropsHandleContext, Task<(int code, object data)>> action = (h, c) => h.ConsumeAsync(c);
            return Handle(propId, num, para, action);
        }

        private Task<(int code, object data)> Handle(int propId, long num, dynamic parameter, Func<IPropsHandler, PropsHandleContext, Task<(int code, object data)>> action)
        {
            PropsHandleContext context = CreateContext(propId, num, parameter);
            var handler = GetHandler(propId);
            var result = action(handler, context);
            return result;

        }
        private PropsHandleContext CreateContext(int propId, long num, dynamic parameter)
        {
            return new PropsHandleContext { PropId = propId, Num = num, Parameter = parameter };
        }
        private IPropsHandler GetHandler(int propId)
        {
            var handler = Providers.Reverse()
                                    .Select(provider => provider.GetHandler(propId))
                                    .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的道具处理器");
            return handler;
        }
    }
}
