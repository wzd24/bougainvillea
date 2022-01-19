using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyMigrator;

using FluentMigrator;

using Sailina.Tang.Essential;

using Scorpio.Bougainvillea.Props;

namespace Sailina.Tang.Databases
{
    /// <summary>
    /// 
    /// </summary>
    public class EssentialMigration
    {

    }
    /// <summary>
    /// 
    /// </summary>
    [TimestampedMigration(2021, 12, 30, 19, 24)]
    public class EssentialMigration_2021_12_30_19_24 : AutoReversingMigration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Up()
        {
            Create.Table<AvatarBaseInfo>();
            Create.Table<AvatarHotData>();
            Create.Table<AvatarCurrency>();
            Create.Table<AvatarColdData>();
            Create.Table<Props>();
            Create.Table<Hero>();
            Create.Table<Beauty>();
            Create.Table<BeautyMisc>();
        }
    }
}
