using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Rewards
{

    /// <summary>
    /// 
    /// </summary>
    public interface IRewardHandler:ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code,object data)> ExecuteAsync(RewardHandleContext context);
    }
}
