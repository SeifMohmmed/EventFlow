using EventFlow.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Modules.Users.Infrastructure.Users;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Map the entity to the "roles" table.
        builder.ToTable("roles");

        // Use the role name as the primary key.
        builder.HasKey(r => r.Name);

        // Limit the role name length.
        builder.Property(r => r.Name).HasMaxLength(50);

        // Configure the many-to-many relationship between users and roles.
        builder
            .HasMany<User>()
            .WithMany(u => u.Roles)
            .UsingEntity(joinBuilder =>
            {
                // Store the relationship in the "user_roles" table.
                joinBuilder.ToTable("user_roles");

                // Rename the generated foreign key column.
                joinBuilder.Property("RolesName").HasColumnName("role_name");
            });

        // Seed the default roles.
        builder.HasData(
            Role.Member,
            Role.Administrator);
    }
}
