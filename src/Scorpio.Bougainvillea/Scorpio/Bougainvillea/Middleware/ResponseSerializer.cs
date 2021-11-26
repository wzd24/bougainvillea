using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Middleware
{
    internal class ResponseSerializer : IResponseSerializer,ISingletonDependency
    {
        private readonly IJsonSerializer _jsonSerializer;

        public ResponseSerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }
        public byte[] Serialize<T>(T value) => Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(value));
    }
}
