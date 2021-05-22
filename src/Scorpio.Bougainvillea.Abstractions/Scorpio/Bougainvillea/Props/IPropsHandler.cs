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
        /// 判断是否足够
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<int> EnoughAsync(PropsHandleContext context);
        /// <summary>
        /// 扣除道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code, object data)> ConsumeAsync(PropsHandleContext context);

        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code, object data)> AddPropAsync(PropsHandleContext context);

        /// <summary>
        /// 判断是否可使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<int> CanUseAsync(PropsHandleContext context);

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code, object data)> UseAsync(PropsHandleContext context);
    }
}
