using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Props
{

    /// <summary>
    /// 道具集合接口。
    /// </summary>
    public interface IPropsSet
    {
        /// <summary>
        /// 从道具集合中获取指定ID的道具，道具不存在则返回 null。
        /// </summary>
        /// <param name="propsId">需要获取的道具的Id</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含获取的道具实例。道具不存在则结果值为 null。</returns>
        Task<Props> GetPropsAsync(int propsId);

        /// <summary>
        /// 从道具集合中添加或扣除指定ID及数量的道具。
        /// </summary>
        /// <param name="propsId">要添加或扣除的道具Id</param>
        /// <param name="num">要添加或扣除的道具数量</param>
        /// <returns>表示异步获取操作的任务。 其 Result 属性的值包含添加或扣除的操作状态及结果。</returns>
        Task<(int code, object data)> AddOrSubtractAsync(int propsId, int num);
    }
}
