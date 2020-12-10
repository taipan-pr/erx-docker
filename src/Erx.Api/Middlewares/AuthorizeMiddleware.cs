using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Erx.Api.Middlewares
{
    public class AuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        public AuthorizeMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestToken = context.Request.Headers["Authorization"].FirstOrDefault();

            if (_memoryCache.TryGetValue("token", out Guid token) && !string.IsNullOrWhiteSpace(requestToken))
            {
                context.Items.Add("token", token.ToString("N"));
                context.Items.Add("requestToken", requestToken);
            }

            await _next(context);
        }
    }
}
