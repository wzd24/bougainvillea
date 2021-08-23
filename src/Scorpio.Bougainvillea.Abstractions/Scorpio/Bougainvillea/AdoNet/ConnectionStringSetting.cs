using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Setting;

namespace Scorpio.Bougainvillea.AdoNet
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConnectionStringSetting : GameSettingBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServerId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
