using Microsoft.AspNetCore.Http;

using Scorpio.Bougainvillea.Middleware;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreGameResponse : IGameResponse
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpResponse _httpResponse;
        private readonly StringBuilder _buffer;

        public int Code { get; set; } = 200;

        public IGameContext Context { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public AspnetCoreGameResponse(IServiceProvider serviceProvider, IGameContext context, HttpResponse httpResponse)
        {
            _serviceProvider = serviceProvider;
            Context = context;
            _httpResponse = httpResponse;
            _buffer = new StringBuilder();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task WriteAsync(string value)
        {
            _buffer.Append(value);
            await Task.CompletedTask;
        }

        public async Task FlushAsync()
        {
            Headers.ForEach(kv =>
            {
                _httpResponse.Headers[kv.Key] = kv.Value;
            });
            await _httpResponse.WriteAsync(_buffer.ToString());
        }

        public async Task ClearAsync()
        {
            _httpResponse.Clear();
            _buffer.Clear();
            await Task.CompletedTask;
        }
    }
}