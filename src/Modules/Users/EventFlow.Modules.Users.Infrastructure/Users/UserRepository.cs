using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        // Attach existing roles so EF Core doesn't try to insert them again.
        foreach (Role role in user.Roles)
        {
            context.Attach(role);
        }

        // Add the user to the database.
        context.Users.Add(user);
    }
}

