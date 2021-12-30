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
        private Avatar _avatar;
        private PropsState _props;

        public PropsSubSystem(IServiceProvider serviceProvider)
        {
            PropsHandleManager = ActivatorUtilities.CreateInstance<PropsHandleManager>(serviceProvider, this as IPropsSet);
            _serviceProvider = serviceProvider;
        }

        public IPropsHandleManager PropsHandleManager { get; }

        ValueTask<int> IPropsSet.AddOrSubtractAsync(int propsId, int num)
        {
            var count = _props.AddOrUpdate(propsId, id => new Props { Count = num, LastGetTime = DateTime.Now, PropsId = propsId, AvatarId = _avatar.Id }, (k, p) => p.Action(pp => pp.Count += num)).Count;
            return ValueTask.FromResult(0);
        }

        ValueTask<Props> IPropsSet.GetPropsAsync(int propsId) => ValueTask.FromResult(_props.GetOrDefault(propsId));

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
            _avatar = avatarBase as Avatar;
            _props = _avatar.State.Props;
            return ValueTask.CompletedTask;
        }
    }
}
