namespace EventFlow.Modules.Events.Application.Abstractions.Data;

/// <summary>
/// Represents the Unit of Work for persisting changes
/// made within the Events module.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
