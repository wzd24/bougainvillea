using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Essential.Dtos.Servers
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EnterResult
    {
        /// <summary>
        /// 
        /// </summary>
        public long AvatarId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CanLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset ServerTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CanRegister { get; set; }

    }
}
