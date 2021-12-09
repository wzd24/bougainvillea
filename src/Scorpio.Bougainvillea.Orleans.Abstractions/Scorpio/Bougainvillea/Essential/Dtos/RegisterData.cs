using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterData
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
        public string AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RegisterIp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int HeadId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SkinId { get; set; }


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

        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
    }
}
