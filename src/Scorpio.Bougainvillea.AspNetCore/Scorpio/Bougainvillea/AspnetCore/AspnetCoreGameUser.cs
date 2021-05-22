using Microsoft.AspNetCore.Http;
using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreGameUser : IGameUser
    {
        private readonly HttpContext _context;

        public AspnetCoreGameUser(HttpContext context)
        {
            _context = context;
        }

        public string Key => _context.User?.Identity?.Name;
    }
}