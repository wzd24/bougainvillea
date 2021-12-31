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
            SqlMapper.AddTypeHandler(new JsonConvertHandler<List<long>>());
            SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<int, int>>());
            SqlMapper.AddTypeHandler(new JsonConvertHandler<Dictionary<int, long>>());
            SqlMapper.AddTypeHandler(new FuncConvertHandler<TimeSpan>(v => TimeSpan.FromMilliseconds(v.To<long>()), (p, t) => (long)t.TotalMilliseconds));
            //SqlMapper.AddTypeHandler(new FuncConvertHandler<DateTimeOffset>(v => DateTimeOffset.FromUnixTimeMilliseconds(v.To<long>()), (p, t) => t.ToUnixTimeMilliseconds()));
            base.Initialize(context);
        }
    }
}
