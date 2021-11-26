using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime;
using Orleans.Streams;

using Sailina.Tang.Essential.Dtos;
using Sailina.Tang.Essential.StreamDatas;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    [ImplicitStreamSubscription(AvatarBase.LoginStreamSubscription)]
    internal partial class Avatar : AvatarBase<Avatar, AvatarState, AvatarBaseInfo>, IAvatar
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="dateTimeProvider"></param>
        public Avatar(IServiceProvider serviceProvider, IDateTimeProvider dateTimeProvider) : base(serviceProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
        }

        public override async Task OnDeactivateAsync()
        {
            await base.OnDeactivateAsync();
        }

        protected override async ValueTask LoginAsync(LoginData d)
        {
            UpdateDeviceInfo(d);
            await UpdateLoginStatus();
            await base.LoginAsync(d);
        }


        private async ValueTask UpdateLoginStatus()
        {
            State.HotData.LastOfflineTime = await _dateTimeProvider.GetNowAsync();
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
        public Task<string> GetAvatarNameAsync()
        {
            return Task.FromResult(State.Base.NickName);
        }
    }

}
