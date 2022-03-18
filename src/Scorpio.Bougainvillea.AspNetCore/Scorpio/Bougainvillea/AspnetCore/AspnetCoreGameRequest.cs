using Microsoft.AspNetCore.Http;
using Scorpio.Bougainvillea.Middleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Scorpio.Bougainvillea.AspnetCore
{
    internal class AspnetCoreGameRequest : IGameRequest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameContext _context;
        private readonly HttpRequest _httpRequest;

        public AspnetCoreGameRequest(IServiceProvider serviceProvider, IGameContext context, HttpRequest httpRequest)
        {
            _serviceProvider = serviceProvider;
            _context = context;
            _httpRequest = httpRequest;
            Headers = _httpRequest.Headers.ToDictionary(k => k.Key, k => k.Value.FirstOrDefault(), StringComparer.InvariantCultureIgnoreCase);
            RequestCode = _httpRequest.Headers["Code"].FirstOrDefault();
            using (var reader = new StreamReader(_httpRequest.Body))
            {
                Content = reader.ReadToEndAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        public IReadOnlyDictionary<string, string> Headers { get; }
        public string RequestCode { get; }
        public string Content { get; set; }
    }
}