using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Converters;

namespace Newtonsoft.Json.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class JTokenExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Serialize<T>(this JToken token)
        {
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            };
            //setting.Error += (o, e) => e.ErrorContext.Handled = true;
            setting.Converters.Add(new UnixDateTimeMillisecondsConverter());
            var ser = JsonSerializer.CreateDefault(setting);
            return token.ToObject<T>(ser);
        }
    }
}
