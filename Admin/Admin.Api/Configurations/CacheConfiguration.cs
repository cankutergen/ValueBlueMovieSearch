using Microsoft.Extensions.Caching.Memory;

namespace Admin.Api.Configurations
{
    public static class CacheConfiguration
    {
        public static MemoryCacheEntryOptions GetCacheOptions()
        {
            return new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
        }
    }
}
