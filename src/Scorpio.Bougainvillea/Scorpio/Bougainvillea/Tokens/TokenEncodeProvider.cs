using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;
using Scorpio.Bougainvillea.Middleware;
using Newtonsoft.Json.Linq;

namespace Scorpio.Bougainvillea.Tokens
{
    internal class TokenEncodeProvider : ITokenEncodeProvider, ISingletonDependency
    {
        private readonly IJsonSerializer _jsonSerializer;

        public TokenEncodeProvider(IJsonSerializer jsonSerializer) => _jsonSerializer = jsonSerializer;

        public T Decode<T>(string value)
        {
            try
            {
                return _jsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(Decrypt(Decompress(Convert.FromBase64String(value)))));
            }
            catch
            {

                return default;
            }
        }

        public string Encode<T>(T value)
        {
            return Convert.ToBase64String(Compression(Encrypt(Encoding.UTF8.GetBytes(_jsonSerializer.Serialize(value)))));
        }

        private byte[] Compression(byte[] sources)
        {
            using (var stream = new MemoryStream())
            {
                using (var zip = new DeflateStream(stream, CompressionMode.Compress, true))
                {
                    zip.Write(sources, 0, sources.Length);
                }
                return stream.ToArray();
            }
        }
        private byte[] Decompress(byte[] sources)
        {
            using (var stream = new MemoryStream(sources))
            {
                using (var zip = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    using (var ms = new MemoryStream())
                    {
                        zip.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
        }

        private byte[] Encrypt(byte[] source)
        {
            var result = new byte[source.Length + 4];
            for (var i = 0; i < 4; i++)
            {
                result[i] = (byte)Random.Shared.Next(0, 255);
            }
            for (var i = 0; i < source.Length; i++)
            {
                var ik = i % 4;
                result[i + 4] = (byte)(source[i] ^ result[ik]);
            }
            return result;
        }

        private byte[] Decrypt(byte[] source)
        {
            var result = new byte[source.Length - 4];
            for (var i = 0; i < result.Length; i++)
            {
                var ik = i % 4;
                result[i] = (byte)(source[i + 4] ^ source[ik]);
            }
            return result;
        }
    }
}
