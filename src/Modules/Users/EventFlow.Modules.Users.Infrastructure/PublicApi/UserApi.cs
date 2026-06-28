using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.GetUser;
using EventFlow.Modules.Users.PublicApi;
using MediatR;
using UserResponse = EventFlow.Modules.Users.PublicApi.UserResponse;

namespace EventFlow.Modules.Users.Infrastructure.PublicApi;

// Implements the Users module's public API by forwarding requests
// to the application's query handlers through MediatR.
internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<UserResponse?> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // Execute the query using the application's request pipeline.
        Result<Application.Users.GetUser.UserResponse> result =
            await sender.Send(new GetUserQuery(userId), cancellationToken);

        // Return null when the requested user does not exist
        // or the operation fails.
        if (result.IsFailure)
        {
            return null;
        }

        // Map the application's response to the public DTO exposed
        // by the module to preserve encapsulation.
        return new UserResponse(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);
    }
}
