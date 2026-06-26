using System.Buffers;
using System.Text.Json;
using EventFlow.Common.Application.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace EventFlow.Common.Infrastructure.Caching;

/// <summary>
/// Provides an implementation of <see cref="ICacheService"/>
/// using <see cref="IDistributedCache"/>.
/// </summary>
internal sealed class CacheService(IDistributedCache cache) : ICacheService
{
    /// <summary>
    /// Retrieves a cached value by its key.
    /// </summary>
    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await cache.GetAsync(key, cancellationToken);

        // Return the cached value if found; otherwise return the default value.
        return bytes is null ? default : Deserialize<T>(bytes);
    }

    /// <summary>
    /// Stores a value in the distributed cache.
    /// </summary>
    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        // Serialize the value before storing it.
        byte[] bytes = Serialize(value);

        return cache.SetAsync(
            key,
            bytes,
            CacheOptions.Create(expiration),
            cancellationToken);
    }

    /// <summary>
    /// Removes a cached value by its key.
    /// </summary>
    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default) =>
        cache.RemoveAsync(key, cancellationToken);

    /// <summary>
    /// Deserializes cached bytes into the specified type.
    /// </summary>
    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    /// <summary>
    /// Serializes an object into a UTF-8 byte array.
    /// </summary>
    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }
}
