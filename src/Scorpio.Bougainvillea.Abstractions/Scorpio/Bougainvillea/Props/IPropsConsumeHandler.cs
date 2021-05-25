using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 扣除道具处理器
    /// </summary>
    public interface IPropsConsumeHandler
    {
        /// <summary>
        /// 扣除道具
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<(int code, object data)> ConsumeAsync(PropsHandleContext context);

    }
}
