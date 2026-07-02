using EventFlow.Common.Domain;

namespace EventFlow.Modules.Users.Application.Abstractions.Identity;

/// <summary>
/// Provides operations for interacting with the identity provider.
/// </summary>
public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsnyc(
        UserModel user,
        CancellationToken cancellationToken = default);
}
