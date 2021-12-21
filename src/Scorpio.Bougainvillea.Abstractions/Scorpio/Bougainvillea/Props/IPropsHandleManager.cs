using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Scorpio.Bougainvillea.Props
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropsHandleManager
    {

        /// <summary>
        /// 判断是否有足够的道具。
        /// </summary>
        /// <param name="propId">道具Id</param>
        /// <param name="num">数量</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含判断的结果代码。</returns>
        ValueTask<int> EnoughAsync(int propId, int num);


        /// <summary>
        /// 消费道具，纯扣减道具数量，不做其他操作。
        /// </summary>
        /// <param name="propId">道具Id</param>
        /// <param name="num">消费的数量，正数</param>
        /// <param name="reason">消费道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含消费道具的处理结果。</returns>
        ValueTask<int> ConsumeAsync(int propId, int num,string reason);


        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="reason">添加道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含添加道具的处理结果。</returns>
        ValueTask<int> AddPropAsync(int propId, int num, string reason);

        /// <summary>
        /// 判断是否可使用
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含判断的结果代码。</returns>
        ValueTask<int> CanUseAsync(int propId, int num, object para = null);

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <param name="reason">使用道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含使用道具的处理结果。</returns>
        ValueTask<(int code, object data)> UseAsync(int propId, int num, string reason, object para = null);
    }
}
