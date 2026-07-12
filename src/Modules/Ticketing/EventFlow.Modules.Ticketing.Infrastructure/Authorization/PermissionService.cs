using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Application.Caching;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.IntegrationEvents;
using MassTransit;

namespace EventFlow.Modules.Ticketing.Infrastructure.Authorization;

// Retrieves and caches user permissions using request/response messaging.
internal sealed class PermissionService(
    IRequestClient<GetUserPermissionsRequest> requestClient,
    ICacheService cacheService) : IPermissionService
{
    // Returned when the requested user cannot be found.
    private static readonly Error NotFound = Error.NotFound(nameof(PermissionService), "The user was not found");

    // Cache user permissions for five minutes.
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        // Try to retrieve the user's permissions from the cache.
        PermissionsResponse? permissionsResponse =
            await cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId));

        if (permissionsResponse is not null)
        {
            return permissionsResponse;
        }

        // Request the user's permissions from the Users module.
        var request = new GetUserPermissionsRequest(identityId);

        Response<PermissionsResponse, Error> response =
            await requestClient.GetResponse<PermissionsResponse, Error>(request);

        // Return the error if the request failed.
        if (response.Is(out Response<Error> errorResponse))
        {
            return Result.Failure<PermissionsResponse>(errorResponse.Message);
        }

        // Cache and return the retrieved permissions.
        if (response.Is(out Response<PermissionsResponse> permissionResponse))
        {
            await cacheService.SetAsync(
                CreateCacheKey(identityId),
                permissionResponse.Message,
                CacheExpiration);

            return permissionResponse.Message;
        }

        // Fallback in case no valid response was received.
        return Result.Failure<PermissionsResponse>(NotFound);
    }

    // Creates a cache key for the specified identity.
    private static string CreateCacheKey(string identityId) => $"user-permissions:{identityId}";
}
