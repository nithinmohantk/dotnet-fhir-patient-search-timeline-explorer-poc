using DooLittle.Health.PatientTimeline.Data.Context;
using DooLittle.Health.PatientTimeline.Data.Entities;
using DooLittle.Health.PatientTimeline.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DooLittle.Health.PatientTimeline.Data.Repositories;

/// <summary>
/// Repository implementation for patient data operations.
/// Provides methods for accessing and managing patient entities.
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public PatientRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Patient?> GetBySyntheaIdAsync(string syntheaId)
    {
        if (string.IsNullOrWhiteSpace(syntheaId))
        {
            throw new ArgumentException("Synthea ID cannot be null or empty.", nameof(syntheaId));
        }

        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.SyntheaId == syntheaId);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Patient patient)
    {
        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        await _context.Patients.AddAsync(patient);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Patient patient)
    {
        if (patient == null)
        {
            throw new ArgumentNullException(nameof(patient));
        }

        _context.Patients.Update(patient);
        await Task.CompletedTask; // EF Core Update doesn't need to be awaited
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id);
    }
}