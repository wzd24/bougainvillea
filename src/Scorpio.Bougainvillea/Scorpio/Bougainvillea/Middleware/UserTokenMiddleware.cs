
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

using Scorpio.Middleware.Pipeline;

namespace Scorpio.Bougainvillea.Middleware
{
    internal class UserTokenMiddleware
    {
        private readonly PipelineRequestDelegate<IGameContext> _next;
        private readonly IDistributedCache _cache;

        public UserTokenMiddleware(PipelineRequestDelegate<IGameContext> next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(IGameContext context)
        {
            var token = context.Request.Headers.GetOrDefault("Token");
            if (!token.IsNullOrWhiteSpace())
            {
                context.User = await GetOrCreateUserAsync(token);
            }
            await _next(context);
        }

        private async Task<User> GetOrCreateUserAsync(string token)
        {
            var cached = await _cache.GetStringAsync(token);
            if (cached != null)
            {

                await _cache.RefreshAsync(token);
                return JsonConvert.DeserializeObject<User>(await _cache.GetStringAsync(token));
            }
            var user = GenerateUser(token);
            await _cache.SetStringAsync(token, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
            return user;
        }

        private User GenerateUser(string token)
        {
            return new User(token);
        }
    }

    internal class User : IGameUser
    {
        public string Token { get; }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int ServerId { get; set; }

        string IGameUser.Key => Token;

        public User(string token)
        {
            Token = token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var value = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split('|');
                var avatarId = int.Parse(value[0]);
                var userId = int.Parse(value[1]);
                var serverId = int.Parse(value[2]);
                Id = avatarId;
                UserId = userId;
                ServerId = serverId;
            }
        }
    }
}
