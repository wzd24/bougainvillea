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
    internal class GenericGrain<TValue> : Grain, IGenericGrain<TValue>
    {
        private TValue _value;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask<TValue> GetValue() => new ValueTask<TValue>(_value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ValueTask SetValue(TValue value)
        {
             _value = value;
            return new ValueTask();
        }
    }
}
