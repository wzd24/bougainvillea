using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Scorpio.Bougainvillea.Mails
{
    internal class OfficialMailManager : IOfficialMailManager
    {
        private readonly IOfficialMailStore _store;
        private readonly ILogger<OfficialMailManager> _logger;
        private readonly IJsonSerializer _serializer;
        private readonly Dictionary<int, MailExceptionState> _exceptions;

        public OfficialMailManager(IOfficialMailStore store, ILogger<OfficialMailManager> logger, IJsonSerializer serializer)
        {
            _store = store;
            _logger = logger;
            _serializer = serializer;
            _exceptions = new Dictionary<int, MailExceptionState>();
        }

        public async Task Add(int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime)
        {
            await AddOrUpdate(0, serverId, title, content, rewards, excludeUsers, sendTime, expireTime);
        }

        public async Task AddOrUpdate(int id, int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime)
        {
            var mail = new OfficialMail
            {
                Id = id,
                Content = content,
                ExcludeUsers = _serializer.Deserialize<int[]>(excludeUsers),
                ExpireTime = expireTime,
                Rewards = rewards,
                SendTime = sendTime,
                ServerId = serverId,
                Title = title
            };
            await _store.Save(mail);
        }

        public Task<OfficialMail> GetMail(int id) => _store.GetMail(id);
        public Task<IDictionary<int, MailExceptionState>> GetMailExceptions()
        {
            return Task.FromResult<IDictionary<int, MailExceptionState>>(_exceptions);
        }

        public async Task<IEnumerable<OfficialMail>> GetMails() => (await _store.GetMails()).Where(m => m.ExpireTime <= DateTime.Now);
        public Task SetMailExceptionState(int mailId, MailExceptionState state)
        {
            _exceptions[mailId] = state;
            return Task.CompletedTask;
        }

        public async Task Update(int id, int serverId, string title, string content, string rewards, string excludeUsers, DateTime sendTime, DateTime expireTime)
        {
            await AddOrUpdate(id, serverId, title, content, rewards, excludeUsers, sendTime, expireTime);
        }

    }
}
