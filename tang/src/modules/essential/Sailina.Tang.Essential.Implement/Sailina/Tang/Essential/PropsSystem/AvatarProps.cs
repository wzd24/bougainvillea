using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

using Scorpio.Bougainvillea.Essential;
using Scorpio.Bougainvillea.Props;

namespace Sailina.Tang.Essential
{
    internal partial class Avatar : IAvatarProps
    {
        internal PropsSubSystem PropsSubSystem => SubSystems.GetOrDefault(nameof(PropsSubSystem)) as PropsSubSystem;

        /// <summary>
        /// 判断是否有足够的道具。
        /// </summary>
        /// <param name="propId">道具Id</param>
        /// <param name="num">数量</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含判断的结果代码。</returns>
        ValueTask<int> IAvatarProps.EnoughAsync(int propId, int num)
        {
            return PropsSubSystem.PropsHandleManager.EnoughAsync(propId, num);
        }


        /// <summary>
        /// 消费道具，纯扣减道具数量，不做其他操作。
        /// </summary>
        /// <param name="propId">道具Id</param>
        /// <param name="num">消费的数量，正数</param>
        /// <param name="reason">消费道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含消费道具的处理结果。</returns>
        ValueTask<int> IAvatarProps.ConsumeAsync(int propId, int num, string reason)
        {
            return PropsSubSystem.PropsHandleManager.ConsumeAsync(propId, num, reason);
        }


        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="reason">添加道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含添加道具的处理结果。</returns>
        ValueTask<int> IAvatarProps.AddPropAsync(int propId, int num, string reason)
        {
            return PropsSubSystem.PropsHandleManager.AddPropAsync(propId, num, reason);
        }

        /// <summary>
        /// 判断是否可使用
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含判断的结果代码。</returns>
        ValueTask<int> IAvatarProps.CanUseAsync(int propId, int num, object para )
        {
            return PropsSubSystem.PropsHandleManager.CanUseAsync(propId, num, para);
        }

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <param name="reason">使用道具原因。</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含使用道具的处理结果。</returns>
        ValueTask<(int code, object data)> IAvatarProps.UseAsync(int propId, int num, string reason, object para )
        {
            return PropsSubSystem.PropsHandleManager.UseAsync(propId, num, reason, para);
        }
    }

    internal partial class AvatarState
    {
        /// <summary>
        /// 
        /// </summary>
        public PropsState Props { get; set; } = new PropsState();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PropsState : Dictionary<int, Props>
    {
        /// <summary>
        /// 
        /// </summary>
        public PropsState()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public PropsState(IDictionary<int, Props> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public PropsState(IEnumerable<KeyValuePair<int, Props>> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public PropsState(IEqualityComparer<int> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public PropsState(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="comparer"></param>
        public PropsState(IDictionary<int, Props> dictionary, IEqualityComparer<int> comparer) : base(dictionary, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public PropsState(IEnumerable<KeyValuePair<int, Props>> collection, IEqualityComparer<int> comparer) : base(collection, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public PropsState(int capacity, IEqualityComparer<int> comparer) : base(capacity, comparer)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PropsState(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
