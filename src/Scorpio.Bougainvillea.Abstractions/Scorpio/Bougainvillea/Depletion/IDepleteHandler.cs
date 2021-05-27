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
    public interface IDepleteHandler : ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code,object data)> ExecuteAsync(DepleteHandleContext context);
    }
}
