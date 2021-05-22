using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.AspnetCore;
using Scorpio.Bougainvillea.Middleware.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Scorpio.Bougainvillea.Middleware;

namespace Scorpio.Bougainvillea.AspnetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameEndPointExtensions
    {
        private const string _endpointRouteBuilder = "__EndpointRouteBuilder";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGameEndPoint(this IApplicationBuilder applicationBuilder) => applicationBuilder.UseMiddleware<GameMiddleware>();
    }
}
