using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Depletion
{

    /// <summary>
    /// 
    /// </summary>
    public interface IDepleteHandler : IScopedDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ValueTask<int> CanExecuteAsync(DepleteHandleContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ValueTask<int> ExecuteAsync(DepleteHandleContext context);
    }
}
