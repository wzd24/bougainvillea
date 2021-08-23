using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 添加道具处理接口
    /// </summary>
    public interface IPropsAddHandler
    {
        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<int> AddPropAsync(PropsHandleContext context);

    }
}
