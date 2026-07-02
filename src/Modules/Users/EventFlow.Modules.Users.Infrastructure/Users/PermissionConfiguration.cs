using EventFlow.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Modules.Users.Infrastructure.Users;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Map the entity to the "permissions" table.
        builder.ToTable("permissions");

        // Use the permission code as the primary key.
        builder.HasKey(p => p.Code);

        // Limit the permission code length.
        builder.Property(p => p.Code).HasMaxLength(100);

        // Seed all available permissions.
        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.GetEvents,
            Permission.SearchEvents,
            Permission.ModifyEvents,
            Permission.GetTicketTypes,
            Permission.ModifyTicketTypes,
            Permission.GetCategories,
            Permission.ModifyCategories,
            Permission.GetCart,
            Permission.AddToCart,
            Permission.RemoveFromCart,
            Permission.GetOrders,
            Permission.CreateOrder,
            Permission.GetTickets,
            Permission.CheckInTicket,
            Permission.GetEventStatistics);

        // Configure the many-to-many relationship between roles and permissions.
        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                // Store the relationship in the "role_permissions" table.
                joinBuilder.ToTable("role_permissions");

                // Seed which permissions belong to each role.
                joinBuilder.HasData(
                    // Member permissions
                    CreateRolePermission(Role.Member, Permission.GetUser),
                    CreateRolePermission(Role.Member, Permission.ModifyUser),
                    CreateRolePermission(Role.Member, Permission.SearchEvents),
                    CreateRolePermission(Role.Member, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Member, Permission.GetCart),
                    CreateRolePermission(Role.Member, Permission.AddToCart),
                    CreateRolePermission(Role.Member, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Member, Permission.GetOrders),
                    CreateRolePermission(Role.Member, Permission.CreateOrder),
                    CreateRolePermission(Role.Member, Permission.GetTickets),
                    CreateRolePermission(Role.Member, Permission.CheckInTicket),

                    // Administrator permissions
                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.GetEvents),
                    CreateRolePermission(Role.Administrator, Permission.SearchEvents),
                    CreateRolePermission(Role.Administrator, Permission.ModifyEvents),
                    CreateRolePermission(Role.Administrator, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Administrator, Permission.ModifyTicketTypes),
                    CreateRolePermission(Role.Administrator, Permission.GetCategories),
                    CreateRolePermission(Role.Administrator, Permission.ModifyCategories),
                    CreateRolePermission(Role.Administrator, Permission.GetCart),
                    CreateRolePermission(Role.Administrator, Permission.AddToCart),
                    CreateRolePermission(Role.Administrator, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Administrator, Permission.GetOrders),
                    CreateRolePermission(Role.Administrator, Permission.CreateOrder),
                    CreateRolePermission(Role.Administrator, Permission.GetTickets),
                    CreateRolePermission(Role.Administrator, Permission.CheckInTicket),
                    CreateRolePermission(Role.Administrator, Permission.GetEventStatistics));
            });
    }

    // Creates a row for the role-permission join table.
    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
