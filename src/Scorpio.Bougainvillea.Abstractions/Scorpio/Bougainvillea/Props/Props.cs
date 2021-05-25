using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Props
    {

        /// <summary>
        /// 道具Id
        /// </summary>
        public int PropsId { get; set; }

        /// <summary>
        /// 道具数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 最后一次获取时间
        /// </summary>
        public DateTime LastGetTime { get; set; }
    }
}
