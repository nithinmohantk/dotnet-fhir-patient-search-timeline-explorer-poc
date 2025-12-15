namespace DooLittle.Health.PatientTimeline.Data.Interfaces;

/// <summary>
/// Unit of Work interface for managing database transactions and repositories.
/// Provides access to all repositories and transaction management.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the patient repository.
    /// </summary>
    IPatientRepository Patients { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a database transaction asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the database transaction.</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RollbackTransactionAsync();
}