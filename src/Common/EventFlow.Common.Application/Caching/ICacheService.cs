namespace EventFlow.Common.Application.Caching;

/// <summary>
/// Defines operations for interacting with the application's cache.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a cached value by its key.
    /// </summary>
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores a value in the cache.
    /// </summary>
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a cached value by its key.
    /// </summary>
    Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default);
}
