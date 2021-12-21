using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Rewards;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea.Mails
{
    internal class UserMailManager : IUserMailManager
    {
        private readonly IOfficialMailProvider _officialMailProvider;
        private readonly IUserMailStore _store;
        private readonly ISettingManager _settingManager;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentServer _currentServer;
        private readonly IRewardHandleManager _rewardHandleManager;
        private readonly IUserLastBuildMailTimeProvider _lastBuildMailTimeProvider;

        public UserMailManager(IOfficialMailProvider officialMailProvider,
                               IUserMailStore store,
                               ISettingManager settingManager,
                               ICurrentUser currentUser,
                               ICurrentServer currentServer,
                               IRewardHandleManager rewardHandleManager,
                               IUserLastBuildMailTimeProvider lastBuildMailTimeProvider)
        {
            _officialMailProvider = officialMailProvider;
            _store = store;
            _settingManager = settingManager;
            _currentUser = currentUser;
            _currentServer = currentServer;
            _rewardHandleManager = rewardHandleManager;
            _lastBuildMailTimeProvider = lastBuildMailTimeProvider;
        }

        public async Task<bool> BuildOfficialMailsAsync()
        {
            var result = false;
            var lastBuildMailTime = await _lastBuildMailTimeProvider.GetLastBuildMailTimeAsync();
            var mails = await _officialMailProvider.GetMailsAsync();
            foreach (var item in mails)
            {
                if ((lastBuildMailTime - item.SendTime).TotalSeconds < -10)
                {
                    if (!item.ExcludeUsers.Contains(_currentUser.AvatarId))
                    {
                        await SendMailAsync(UserMailType.System, item.Title, item.Content, item.ExceptionState, item.Rewards, item.ExpireTime);
                    }
                    lastBuildMailTime = item.SendTime;
                    result = true;
                }
            }
            await _lastBuildMailTimeProvider.SetLastBuildMailTimeAsync(lastBuildMailTime);
            return result;
        }

        public async Task<int> CanReadMailAsync(int mailId)
        {
            var mail = await _store.GetMailAsync(mailId);
            return CanReadMail(mail);
        }

        private static int CanReadMail(UserMail mail)
        {
            return mail switch
            {
                null => MailErrorCodes.NotExists,
                { ReadState: true } => MailErrorCodes.MailReaded,
                { ExceptionState: MailExceptionState.Forbid } => MailErrorCodes.NotExists,
                UserMail m when m.ClientExpireTime < DateTime.Now => MailErrorCodes.Expired,
                UserMail m when m.ExpireTime < DateTime.Now => MailErrorCodes.Expired,
                _ => SystemErrorCodes.Success
            };
        }

        public async Task DeleteMailAsync(int mailId)
        {
            await _store.DeleteAsync(mailId);
        }

        public async Task DeleteReadedMailsAsync(UserMailType mailType)
        {
            var mails = (await _store.GetMailsAsync()).Where(m => m.Type == mailType && m.ReadState);
            foreach (var item in mails)
            {
                await DeleteMailAsync(item.Id);
            }
        }

        public async Task<(int code, object data)> ReadAllMailsAsync(UserMailType mailType)
        {
            var mails = await _store.GetMailsAsync();
            var result = new Dictionary<int, object>();
            foreach (var item in mails)
            {
                var (_, data) = await ReadMailAsync(item);
                result.Add(item.Id, data);
            }
            return (SystemErrorCodes.Success, result);
        }

        public async Task<(int code, object data)> ReadMailAsync(int mailId)
        {
            var mail = await _store.GetMailAsync(mailId);
            return await ReadMailAsync(mail);
        }

        public async Task<(int code, object data)> ReadMailAsync(UserMail mail)
        {
            var code = CanReadMail(mail);
            if (code != SystemErrorCodes.Success)
            {
                return (code, null);
            }
            mail.ReadState = true;
            mail.ReadTime = DateTime.Now;
            await _store.SaveAsync(mail);
            return await _rewardHandleManager.HandleAsync(mail.Rewards, 1, $"领取邮件 {mail.Id} 的奖励");
        }


        public Task SendMailAsync(UserMailType type, string title, string content, MailExceptionState state, string rewards, DateTime expireTime)
        {
            return SendMailAsync(type, 0, title, content, state, rewards, expireTime);
        }

        private async Task SendMailAsync(UserMailType type, int resourceId, string title, string content, MailExceptionState state, string rewards, DateTime expireTime)
        {
            var mail = new UserMail
            {
                AvatarId = _currentUser.AvatarId,
                ClientExpireTime = DateTime.Now.AddHours(await _settingManager.GetAsync<int>(SettingDefinitionConsts.MailClientExpireHours)),
                Content = content,
                ExceptionState = state,
                ExpireTime = expireTime,
                Rewards = rewards,
                Title = title,
                Type = type,
                ServerId = _currentServer.ServerId,
                ResourceId = resourceId,
            };
            await _store.SaveAsync(mail);
        }

        public async Task UpdateOfficialMailExceptionAsync()
        {
            var exceptions = await _officialMailProvider.GetMailExceptionsAsync();
            foreach (var item in exceptions)
            {
                var mail = (await _store.GetMailsAsync()).FirstOrDefault(m => m.ResourceId == item.Key);
                if (mail == null)
                {
                    continue;
                }
                var oMail = await _officialMailProvider.GetMailAsync(item.Key);
                if (mail.UpdateTime >= oMail.UpdateTime && mail.ExceptionState == oMail.ExceptionState)
                {
                    continue;
                }
                switch (oMail.ExceptionState)
                {
                    case MailExceptionState.Normal:
                    case MailExceptionState.Forbid:
                        mail.UpdateTime = DateTime.Now;
                        mail.ExceptionState = oMail.ExceptionState;
                       await _store.SaveAsync(mail);
                        break;
                    case MailExceptionState.Deleted:
                        await DeleteMailAsync(mail.Id);
                        break;
                    case MailExceptionState.Modified:
                        mail.UpdateTime = DateTime.Now;
                        mail.ExceptionState = oMail.ExceptionState;
                        mail.Title = oMail.Title;
                        mail.Content = oMail.Content;
                        mail.Rewards = oMail.Rewards;
                      await  _store.SaveAsync(mail);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
