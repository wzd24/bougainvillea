using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Concurrency;

using Sailina.Tang.Essential.Dtos;

using Scorpio.Bougainvillea.Essential.Dtos;
using Scorpio.Bougainvillea.Essential.Dtos.Servers;
using Scorpio.Bougainvillea.Tokens;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServerBase : IGrainWithIntegerKey, IGrainBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> CloseAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> MaintenanceAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<ServerStatus> OpenAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<DateTimeOffset> GetServerTimeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverTimeOffset"></param>
        /// <returns></returns>
        ValueTask SetServerTimeOffset(TimeSpan serverTimeOffset);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask BeginInitializeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        ValueTask<AvatarInfo> GetAvatarAsync(int avatarId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OneWay]
        ValueTask BeginLoginAsync(LoginData request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        ValueTask<EnterResult> CheckUserAsync(UserData userData);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        ValueTask<int> GenerateAvatarAsync(RegisterData generateInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ValueTask<(int code, AvatarInfo result)> EndGenerateAvatarAsync(long userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        ValueTask<bool> IsGeneratedAsync(long avatarId);
    }
}
