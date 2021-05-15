using Microsoft.Extensions.Options;

using Scorpio.DependencyInjection;

using System;
using System.Collections.Generic;
namespace Scorpio.Bougainvillea.Handler
{
    /// <summary>
    /// 
    /// </summary>
    internal class DefaultGameHandlerFactory : IGameHandlerFactory, ISingletonDependency
    {
        private readonly Dictionary<string, GameHandleDelegate> _handlers = new Dictionary<string, GameHandleDelegate>();
        private readonly GameHandlerOptions _options;

        public DefaultGameHandlerFactory(IServiceProvider serviceProvider, IOptions<GameHandlerOptions> options)
        {
            _options = options.Value;
            ServiceProvider = serviceProvider;
            _options.ApplyHanders(this);
        }

        public IServiceProvider ServiceProvider { get; }

        public void AddHandler(string code, GameHandleDelegate handler)
        {
            _handlers.Add(code, handler);
        }

        public GameHandleDelegate Create(string key)
        {
            return _handlers.GetOrDefault(key);
        }
    }
}
