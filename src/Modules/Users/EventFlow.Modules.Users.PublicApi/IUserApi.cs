namespace EventFlow.Modules.Users.PublicApi;

// Defines the public contract exposed by the Users module.
// Other modules depend on this abstraction instead of referencing
// the Users application's internal implementation.
public interface IUsersApi
{
    Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}
