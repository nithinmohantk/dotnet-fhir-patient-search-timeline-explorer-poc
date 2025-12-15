using DooLittle.Health.PatientTimeline.Data.Context;
using DooLittle.Health.PatientTimeline.Data.Interfaces;

namespace DooLittle.Health.PatientTimeline.Data.Repositories;

/// <summary>
/// Unit of Work implementation for managing database transactions and repositories.
/// Provides access to all repositories and transaction management.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IPatientRepository? _patients;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IPatientRepository Patients => _patients ??= new PatientRepository(_context);

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    /// <summary>
    /// Disposes the unit of work and underlying database context.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}