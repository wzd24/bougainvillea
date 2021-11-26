
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
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentServer _currentServer;

        public UserTokenMiddleware(PipelineRequestDelegate<IGameContext> next, ICurrentUser currentUser, ICurrentServer currentServer)
        {
            _next = next;
            _currentUser = currentUser;
            _currentServer = currentServer;
        }

        public async Task InvokeAsync(IGameContext context)
        {
            var token = context.Request.Headers.GetOrDefault("Token");
            if (!token.IsNullOrWhiteSpace())
            {
                context.User = await GetOrCreateUserAsync(token);
                using (_currentUser.Use(context.User.Id))
                {
                    using (_currentServer.Use(context.User.ServerId))
                    {
                        await _next(context);

                    }
                }
            }
            else
            {
                await _next(context);
            }
        }

        private Task<User> GetOrCreateUserAsync(string token)
        {
            User user = null;
            //if (cached != null)
            //{
            //    await _cache.RefreshAsync(token);
            //    user = JsonConvert.DeserializeObject<User>(await _cache.GetStringAsync(token));
            //    if (user != null)
            //    {
            //        if (user.IsValid && user.Expiration.Subtract(DateTime.Now).TotalHours < 1)
            //        {
            //            user.Expiration = DateTime.Now.AddHours(2);
            //        }
            //        else
            //        {
            //            return user;
            //        }
            //    }
            //}
            if (user == null)
            {
                user = GenerateUser(token);
            }
            //await _cache.SetStringAsync(token, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(2)));
            return Task.FromResult(user);
        }

        private User GenerateUser(string token)
        {
            return new User(token);
        }
    }

    [Serializable]
    internal class User : IGameUser
    {
        public string Token { get; }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int ServerId { get; set; }

        public DateTime Expiration { get; set; }

        string IGameUser.Key => Token;

        public bool IsValid { get => Id != 0 && ServerId != 0 && Expiration > DateTime.Now; }

        public User()
        {

        }
        public User(string token)
        {
            Token = token;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var value = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split('|');
                var avatarId = int.Parse(value[0]);
                var userId = int.Parse(value[1]);
                var serverId = int.Parse(value[2]);
                var expiration = DateTime.Parse(value[3]);
                Id = avatarId;
                UserId = userId;
                ServerId = serverId;
                Expiration = expiration;
            }
        }
    }
}
