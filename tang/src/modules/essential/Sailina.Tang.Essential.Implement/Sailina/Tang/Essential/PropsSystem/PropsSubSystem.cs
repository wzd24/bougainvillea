using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Props;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Essential
{
    internal class PropsSubSystem : ISubSystem, IPropsSet, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private PropsState _props;

        public PropsSubSystem(IServiceProvider serviceProvider)
        {
            PropsHandleManager = ActivatorUtilities.CreateInstance<PropsHandleManager>(serviceProvider, this as IPropsSet);
            _serviceProvider = serviceProvider;
        }

        public IPropsHandleManager PropsHandleManager { get; }

        public ValueTask<int> AddOrSubtractAsync(int propsId, int num)
        {
            var count = _props.GetOrAdd(propsId, id => new Props { Count = 0, LastGetTime = DateTime.Now, PropsId = propsId }).Count += num;
            return ValueTask.FromResult(0);
        }

        public ValueTask<Props> GetPropsAsync(int propsId) => ValueTask.FromResult(_props.GetOrDefault(propsId));

        public ValueTask InitializeAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarBase"></param>
        /// <returns></returns>
        public ValueTask OnSetupAsync(IAvatarBase avatarBase)
        {
            var ava = avatarBase as Avatar;
            _props = ava.State.Props;
            return ValueTask.CompletedTask;
        }
    }
}
