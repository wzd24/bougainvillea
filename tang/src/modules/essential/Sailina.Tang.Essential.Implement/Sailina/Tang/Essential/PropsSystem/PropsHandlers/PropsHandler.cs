using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Props;
using Scorpio.EventBus;

namespace Sailina.Tang.Essential.PropsHandlers
{
    internal class PropsHandler : IPropsHandler
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IGrainActivationContext _grainActivationContext;

        public PropsHandler(IJsonSerializer jsonSerializer,IGrainActivationContext grainActivationContext)
        {
            _jsonSerializer = jsonSerializer;
            _grainActivationContext = grainActivationContext;
        }
        public Task<(int code, object data)> UseAsync(PropsHandleContext context)
        {
            var ava = _grainActivationContext.GrainInstance as Avatar;
            ava.State.Base.Level = 99;
            ava.State.Base.NickName = "张三";
            return Task.FromResult<(int,object)>((0, null));
        }
    }
}
