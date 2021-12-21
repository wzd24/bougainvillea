using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Props
{

    /// <summary>
    /// 
    /// </summary>
    public interface IPropsHandler : ITransientDependency
    {


        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code, object data)> UseAsync(PropsHandleContext context);
    }
}
