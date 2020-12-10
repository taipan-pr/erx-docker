using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Erx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public TokenController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult GetToken()
        {
            if (_memoryCache.TryGetValue("token", out Guid token)) return Ok(token);

            token = Guid.NewGuid();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _memoryCache.Set("token", token, cacheEntryOptions);

            return Ok(token);
        }
    }
}
