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
        /// <returns></returns>
        Task<int> EnoughAsync(int propId, long num);

        /// <summary>
        /// 判断是否有足够的道具
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        Task<(int,Dictionary< int,int>)> EnoughAsync(Dictionary<int, long> props);

        /// <summary>
        /// 消费道具，纯扣减道具数量，不做其他操作。
        /// </summary>
        /// <param name="propId">道具Id</param>
        /// <param name="num">消费的数量，正数</param>
        /// <returns></returns>
        Task<(int code, object data)> ConsumeAsync(int propId, long num);

        /// <summary>
        /// 消费道具，纯扣减道具数量，不做其它操作。
        /// </summary>
        /// <param name="props">道具Id, 消费的数量正数</param>
        /// <returns></returns>
        Task<(int,Dictionary<int, (int code, object data)>)> ConsumeAsync(Dictionary<int, long> props);

        /// <summary>
        /// 添加道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        Task<(int code, object data)> AddPropAsync(int propId, long num);

        /// <summary>
        /// 判断是否可使用
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<int> CanUseAsync(int propId, long num, dynamic para = null);

        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="propId"></param>
        /// <param name="num"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<(int code, object data)> UseAsync(int propId, int num, dynamic para = null);
    }
}
