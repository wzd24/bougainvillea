using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sailina.Tang.Essential.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long AvatarId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LoginIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DeviceInfo DeviceInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AppInfo AppInfo { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceOS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceVer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OsLanguage { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string CVer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlatformID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SmallPlatformID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppId { get; set; }

    }
}
