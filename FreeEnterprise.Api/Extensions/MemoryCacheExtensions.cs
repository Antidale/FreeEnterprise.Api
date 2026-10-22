using Microsoft.Extensions.Caching.Memory;

namespace FreeEnterprise.Api.Extensions;

public static class IMemoryCacheExtensions
{
    /// <summary>
    /// An override of the base Set method, defaulting to a 5 minute timeout on cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="cacheKey"></param>
    /// <param name="cacheItem"></param>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    public static T SetCache<T>(this IMemoryCache cache, string cacheKey, T cacheItem, TimeSpan? timeSpan = null)
    {
        timeSpan ??= TimeSpan.FromMinutes(5);
        return cache.Set(cacheKey, cacheItem,
            new MemoryCacheEntryOptions().SetSlidingExpiration(timeSpan.Value));
    }
}
