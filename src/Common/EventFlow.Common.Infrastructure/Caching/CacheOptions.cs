using Microsoft.Extensions.Caching.Distributed;

namespace EventFlow.Common.Infrastructure.Caching;

/// <summary>
/// Provides helper methods for creating distributed cache options.
/// </summary>
public static class CacheOptions
{
    /// <summary>
    /// Gets the default cache expiration time.
    /// </summary>
    public static DistributedCacheEntryOptions DefaultExpiration => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };

    /// <summary>
    /// Creates cache options using the specified expiration time,
    /// or the default expiration if none is provided.
    /// </summary>
    /// <param name="expiration">
    /// The cache lifetime. If <c>null</c>, the default expiration is used.
    /// </param>
    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration is not null
            ? new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            }
            : DefaultExpiration;
}
