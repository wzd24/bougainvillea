using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Scorpio.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Bougainvillea
{
    internal class JsonSerializer : IJsonSerializer,ISingletonDependency
    {
        private readonly JsonSerializerSettings _settings;
        public JsonSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            };
            _settings.Converters.Add(new UnixDateTimeConverter());
        }
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}
