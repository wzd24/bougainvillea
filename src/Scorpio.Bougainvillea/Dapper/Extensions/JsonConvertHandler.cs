using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Dapper.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonConvertHandler<T> : SqlMapper.TypeHandler<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>((string)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FuncConvertHandler<T> : SqlMapper.TypeHandler<T>
    {
        private readonly Func<object,T> _parseFunc;
        private readonly Func<IDbDataParameter,T, object> _setFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setFunc"></param>
        /// <param name="parseFunc"></param>
        public FuncConvertHandler(Func<object, T> parseFunc, Func<IDbDataParameter,T, object> setFunc)
        {
            _setFunc = setFunc;
            _parseFunc = parseFunc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override T Parse(object value) => _parseFunc(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public override void SetValue(IDbDataParameter parameter, T value) => _setFunc(parameter,value);
    }
}
