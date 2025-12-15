using DooLittle.Health.PatientTimeline.Data.Entities;

namespace DooLittle.Health.PatientTimeline.Data.Interfaces;

/// <summary>
/// Repository interface for patient data operations.
/// Defines methods for accessing and managing patient entities.
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Gets all patients asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of patients.</returns>
    Task<IEnumerable<Patient>> GetAllAsync();

    /// <summary>
    /// Gets a patient by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the patient if found, null otherwise.</returns>
    Task<Patient?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a patient by their Synthea identifier asynchronously.
    /// </summary>
    /// <param name="syntheaId">The Synthea identifier of the patient.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the patient if found, null otherwise.</returns>
    Task<Patient?> GetBySyntheaIdAsync(string syntheaId);

    /// <summary>
    /// Adds a new patient asynchronously.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(Patient patient);

    /// <summary>
    /// Updates an existing patient asynchronously.
    /// </summary>
    /// <param name="patient">The patient to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Patient patient);

    /// <summary>
    /// Deletes a patient by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the patient was deleted, false otherwise.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Checks if a patient exists by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the patient exists, false otherwise.</returns>
    Task<bool> ExistsAsync(int id);
}