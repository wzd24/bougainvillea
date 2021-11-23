using System;
using System.Collections.Generic;

using Dapper;
using Dapper.Extensions;

using Scorpio.Modularity;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    public class BougainvilleaStoragesModule:ScorpioModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(ApplicationInitializationContext context)
        {
            SqlMapper.AddTypeHandler(new JsonConvertHandler<List<int>>());
            base.Initialize(context);
        }
    }
}
