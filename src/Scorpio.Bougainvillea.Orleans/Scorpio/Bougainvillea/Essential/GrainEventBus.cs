using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Scorpio.Data;
using Scorpio.DependencyInjection;
using Scorpio.EventBus;
using Scorpio.Threading;

namespace Scorpio.Bougainvillea.Essential
{
    internal class GrainEventBus : EventBusBase
    {
        public GrainEventBus(IServiceProvider serviceProvider, IEventErrorHandler errorHandler, IOptions<EventBusOptions> options) : base(serviceProvider, errorHandler, options)
        {
        }

        public override async Task PublishAsync(Type eventType, object eventData) => await PublishAsync(new LocalEventMessage(Guid.NewGuid(), eventData, eventType));

        public virtual async Task PublishAsync(LocalEventMessage localEventMessage)
        {
            await TriggerHandlersAsync(localEventMessage.EventType, localEventMessage.EventData, errorContext =>
            {
                errorContext.EventData = localEventMessage.EventData;
                errorContext.SetProperty(nameof(LocalEventMessage.MessageId), localEventMessage.MessageId);
            });
        }
        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                           .Locking(factories => factories.Add(factory));
            return new DisposeAction(() => Unsubscribe(eventType, factory));
        }
       

        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory) => GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));

        public override void UnsubscribeAll(Type eventType) => GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());

    }
}
