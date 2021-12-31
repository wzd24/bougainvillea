using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

using Sailina.Tang.Essential.Dtos;
using Sailina.Tang.Essential.Settings;
using Sailina.Tang.Essential.StreamDatas;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    internal partial class Avatar : AvatarBase<Avatar, AvatarState, AvatarBaseInfo>, IAvatar
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="dateTimeProvider"></param>
        /// <param name="options"></param>
        public Avatar(IServiceProvider serviceProvider, IDateTimeProvider dateTimeProvider, IOptions<AvatarOptions> options) : base(serviceProvider, options)
        {
            _dateTimeProvider = dateTimeProvider;
        }


        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            State.ColdData.AvatarId = State.HotData.AvatarId = State.Currency.AvatarId = State.Beauties.Misc.AvatarId = State.Base.Id = Id;
        }

        public override async Task OnDeactivateAsync()
        {
            await base.OnDeactivateAsync();
        }

        protected override async ValueTask LoginAsync(LoginData d)
        {
            UpdateDeviceInfo(d);
            await UpdateLoginStatus(d);
            await base.LoginAsync(d);
        }


        private async ValueTask UpdateLoginStatus(LoginData d)
        {
            State.HotData.LastOfflineTime = (await _dateTimeProvider.GetNowAsync()).DateTime;
            State.ColdData.LoginIp = d.LoginIp;
            State.HotData.LoginTimes++;
            State.HotData.LoginStatus = true;
        }

        private void UpdateDeviceInfo(LoginData d)
        {
            State.ColdData.AppId = d.AppInfo.AppId;
            State.ColdData.AppVersion = d.AppInfo.CVer;
            State.ColdData.DeviceID = d.DeviceInfo.DeviceID;
            State.ColdData.DeviceOS = d.DeviceInfo.DeviceOS;
            State.ColdData.DeviceType = d.DeviceInfo.DeviceType;
            State.ColdData.DeviceVer = d.DeviceInfo.DeviceVer;
            State.ColdData.OSLanguage = d.DeviceInfo.OsLanguage;
            State.ColdData.SmallPlatformID = d.AppInfo.SmallPlatformID;
            State.ColdData.PlatformID = d.AppInfo?.PlatformID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<string> IAvatar.GetAvatarNameAsync()
        {
            return ValueTask.FromResult(State.Base.NickName);
        }

        public override ValueTask<IDictionary<string, object>> GetLoginInfoAsync() => throw new NotImplementedException();

        protected override async ValueTask<bool> NeedDailyReset()
        {
            var date = (await _dateTimeProvider.GetNowAsync()).Date;
            if (State.HotData.ResetTime < date)
            {
                State.HotData.ResetTime = (await _dateTimeProvider.GetNowAsync()).DateTime;
                return true;
            }
            return false;
        }

        public async ValueTask<int> AddHead(int headId, string reason)
        {
            var setting = (await GameSettingManager.GetAsync<HeadSetting>()).SingleOrDefault(s => s.Id == headId);
            if (setting == null)
            {
                return (int)Avatars.ErrorCode.ConfigurationDoesNotExist;
            }
            if (!State.Base.HeadIds.Contains(headId))
            {
                State.Base.HeadIds.Add(headId);
            }
            return 0;
        }

        public async ValueTask SaveAsync()
        {
            await AvatarState.WriteStateAsync();
        }
    }

}
