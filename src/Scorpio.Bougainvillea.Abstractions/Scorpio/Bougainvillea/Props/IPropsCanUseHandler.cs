using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 判断道具是否可使用处理接口
    /// </summary>
    public interface IPropsCanUseHandler
    {
        /// <summary>
        /// 判断是否可使用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code,Props props)> CanUseAsync(PropsHandleContext context);

    }
}
