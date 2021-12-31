using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using EasyMigrator;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Props
    {
        /// <summary>
        /// 所属玩家Id
        /// </summary>
        [ExplicitKey]
       [Pk]
        [Index]
        public virtual long AvatarId { get; set; }
        /// <summary>
        /// 道具Id
        /// </summary>
        [ExplicitKey]
        [Pk]
        public virtual int PropsId { get; set; }

        /// <summary>
        /// 道具数量
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// 最后一次获取时间
        /// </summary>
       [DbType(System.Data.DbType.DateTime)]
        public virtual DateTime LastGetTime { get; set; }
    }
}
