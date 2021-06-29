using Microsoft.AspNetCore.Http;

using Scorpio.Bougainvillea.Middleware;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreGameResponse : IGameResponse
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpResponse _httpResponse;
        private readonly List<byte> _buffer;

        public int Code { get; set; } = 200;

        public IGameContext Context { get; }
        public IDictionary<string, string> Headers { get; }

        public AspnetCoreGameResponse(IServiceProvider serviceProvider, IGameContext context, HttpResponse httpResponse)
        {
            _serviceProvider = serviceProvider;
            Context = context;
            _httpResponse = httpResponse;
            Headers = new HeadersWrapper(_httpResponse.Headers);
            _buffer =  new List<byte>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task WriteAsync(ReadOnlyMemory<byte> value)
        {           
            _buffer.AddRange(value.ToArray());
            await Task.CompletedTask;
        }

        public async Task FlushAsync()
        {
            Headers.ForEach(kv =>
            {
                _httpResponse.Headers[kv.Key] = kv.Value;
            });
            await _httpResponse.BodyWriter.WriteAsync(_buffer.ToArray());
        }

        public async Task ClearAsync()
        {
            _httpResponse.Clear();
            _buffer.Clear();
            await Task.CompletedTask;
        }
    }

    internal class HeadersWrapper : IDictionary<string, string>
    {
        private readonly IHeaderDictionary _inner;

        public HeadersWrapper(IHeaderDictionary inner)
        {
            _inner = inner;
        }
        public string this[string key] { get=>_inner[key]; set=>_inner[key]=value; }

        public ICollection<string> Keys => _inner.Keys;
        public ICollection<string> Values => _inner.Values.Cast<string>().ToList();
        public int Count => _inner.Count;
        public bool IsReadOnly { get; }

        public void Add(string key, string value) => _inner.Add(key,value);
        public void Add(KeyValuePair<string, string> item) => _inner.Add(item.Key,item.Value);
        public void Clear() => _inner.Clear();
        public bool Contains(KeyValuePair<string, string> item) => _inner.Contains(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(item.Key,item.Value));
        public bool ContainsKey(string key) => _inner.ContainsKey(key);
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => throw new NotImplementedException();
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _inner.Select(e=>new KeyValuePair<string, string>(e.Key,e.Value)).GetEnumerator();
        public bool Remove(string key) => _inner.Remove(key);
        public bool Remove(KeyValuePair<string, string> item) => throw new NotImplementedException();
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            var result= _inner.TryGetValue(key, out var value1);
            value = value1;
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}