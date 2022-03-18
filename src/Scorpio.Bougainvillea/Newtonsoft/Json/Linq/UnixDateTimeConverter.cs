using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Newtonsoft.Json.Converters;

namespace Newtonsoft.Json.Linq
{

    /// <summary>
    /// Converts a System.DateTime to and from Unix epoch time
    /// </summary>
    public class UnixDateTimeMillisecondsConverter : DateTimeConverterBase
    {
        internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="JsonSerializationException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long num;
            if (value is DateTime time)
            {
                num = (long)(time.ToUniversalTime() - UnixEpoch).TotalSeconds;
            }
            else
            {
                if (!(value is DateTimeOffset))
                {
                    throw new JsonSerializationException("Expected date object value.");
                }

                num = (long)(((DateTimeOffset)value).ToUniversalTime() - UnixEpoch).TotalMilliseconds;
            }

            if (num < 0)
            {
                throw new JsonSerializationException("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.");
            }

            writer.WriteValue(num);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType"> Type of the object.</param>
        /// <param name="existingValue">The existing property value of the JSON that is being converted.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            bool flag = (!objectType.IsValueType) || (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>));
            if (reader.TokenType == JsonToken.Null)
            {

                return null;
            }

            long result;
            if (reader.TokenType == JsonToken.Integer)
            {
                result = (long)reader.Value;
            }
            else
            {
                return null;
            }

            if (result >= 0)
            {
                DateTime unixEpoch = UnixEpoch;
                DateTime dateTime = unixEpoch.AddMilliseconds(result);
                if ((flag ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof(DateTimeOffset))
                {
                    return new DateTimeOffset(dateTime, TimeSpan.Zero);
                }

                return dateTime;
            }
            return null;
        }
    }
}
