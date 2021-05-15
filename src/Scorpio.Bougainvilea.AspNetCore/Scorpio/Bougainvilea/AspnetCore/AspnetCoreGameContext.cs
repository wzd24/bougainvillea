using Microsoft.Extensions.DependencyInjection;
using Scorpio.Bougainvillea.Middleware;
using System;
using System.Collections.Generic;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreGameContext : IGameContext
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _ctx;

        public AspnetCoreGameContext(IServiceProvider serviceProvider, Microsoft.AspNetCore.Http.HttpContext ctx)
        {
            _ctx = ctx;
            ApplicationServices = serviceProvider;
            Request = ActivatorUtilities.CreateInstance<AspnetCoreGameRequest>(serviceProvider, this, ctx.Request);
            Response = ActivatorUtilities.CreateInstance<AspnetCoreGameResponse>(serviceProvider, this, ctx.Response);
            User = new AspnetCoreGameUser(ctx);
            Items = new Dictionary<object, object>();
        }

        public IServiceProvider ApplicationServices { get; }
        public IGameRequest Request { get; }
        public IGameResponse Response { get; }
        public IGameUser User { get; set; }
        public IDictionary<object, object> Items { get; }
    }
}