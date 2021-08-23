using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Scorpio.Bougainvillea.Middleware;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class GameContextAccessor : IGameContextAccessor, ISingletonDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IGameContext GameContext
        {
            get => _httpContextAccessor.HttpContext.Items.GetOrDefault("GameContext") as IGameContext;
            set => _httpContextAccessor.HttpContext.Items["GameContext"] = value;
        }
    }
}
