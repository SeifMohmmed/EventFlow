namespace EventFlow.Modules.Users.Domain.Users;

public sealed class Role
{
    // Predefined administrator role.
    public static readonly Role Administrator = new("Administrator");

    // Default role assigned to regular users.
    public static readonly Role Member = new("Member");

    // Creates a role with the specified name.
    private Role(string name)
    {
        Name = name;
    }

    // Required by EF Core when materializing entities.
    private Role()
    {
    }

    // The unique role name.
    public string Name { get; private set; }
}
