using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Extensions;

public static class IMemoryCacheExtensions
{
    public static T SetCache<T>(this IMemoryCache cache, string cacheKey, T cacheItem, TimeSpan? timeSpan = null)
    {
        timeSpan ??= TimeSpan.FromMinutes(5);
        return cache.Set(cacheKey, cacheItem,
            new MemoryCacheEntryOptions().SetSlidingExpiration(timeSpan.Value));
    }
}
