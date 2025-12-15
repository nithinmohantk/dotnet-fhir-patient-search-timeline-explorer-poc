using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a medical observation or lab result for a patient.
/// Contains observation details, values, and relationships to patients and encounters.
/// </summary>
public class Observation
{
    /// <summary>
    /// Gets or sets the unique identifier for the observation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date of the observation.
    /// </summary>
    public DateTime Date { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient who had this observation.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the encounter where this observation was made.
    /// </summary>
    public int? EncounterId { get; set; }

    // Observation details

    /// <summary>
    /// Gets or sets the coding system used for the observation code.
    /// </summary>
    public string? System { get; set; }

    /// <summary>
    /// Gets or sets the observation code from the coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the observation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the value of the observation.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the units of the observation value.
    /// </summary>
    public string? Units { get; set; }

    /// <summary>
    /// Gets or sets the type of the observation value.
    /// </summary>
    public string? Type { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient who had this observation.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the encounter where this observation was made.
    /// </summary>
    public Encounter? Encounter { get; set; }
}