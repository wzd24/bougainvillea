using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IGenericGrain<TValue>:IGrainWithIntegerKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask<TValue> GetValue();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask SetValue(TValue value);
    }
}
