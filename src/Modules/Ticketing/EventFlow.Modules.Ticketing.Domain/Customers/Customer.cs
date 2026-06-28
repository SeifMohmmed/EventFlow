using EventFlow.Common.Domain;

namespace EventFlow.Modules.Ticketing.Domain.Customers;

public sealed class Customer : Entity
{
    // Required by EF Core for materializing entities from the database.
    private Customer()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    // Factory method to ensure a Customer is always created
    // with all required data initialized.
    public static Customer Create(Guid id, string email, string firstName, string lastName)
    {
        return new Customer
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    // Allows updating mutable customer information while keeping
    // entity state changes encapsulated within the aggregate.
    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
