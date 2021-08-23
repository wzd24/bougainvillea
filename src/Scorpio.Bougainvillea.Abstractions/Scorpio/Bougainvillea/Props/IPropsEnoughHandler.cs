using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 判断道具是否足够处理接口
    /// </summary>
    public interface IPropsEnoughHandler
    {
        /// <summary>
        /// 判断是否足够
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<int> EnoughAsync(PropsHandleContext context);

    }
}
