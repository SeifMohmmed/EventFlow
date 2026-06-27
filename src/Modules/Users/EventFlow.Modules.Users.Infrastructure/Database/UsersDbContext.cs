using EventFlow.Modules.Users.Application.Abstractions.Data;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
