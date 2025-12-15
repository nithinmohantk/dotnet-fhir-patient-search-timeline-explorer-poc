using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a medical condition or diagnosis for a patient.
/// Contains condition details, timing, and relationships to patients and encounters.
/// </summary>
public class Condition
{
    /// <summary>
    /// Gets or sets the unique identifier for the condition.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the start date of the condition.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the stop date of the condition (nullable if ongoing).
    /// </summary>
    public DateTime? Stop { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient who has this condition.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the encounter where this condition was diagnosed.
    /// </summary>
    public int? EncounterId { get; set; }

    // Condition details

    /// <summary>
    /// Gets or sets the coding system used for the condition code.
    /// </summary>
    public string? System { get; set; }

    /// <summary>
    /// Gets or sets the condition code from the coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the condition.
    /// </summary>
    public string? Description { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient who has this condition.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the encounter where this condition was diagnosed.
    /// </summary>
    public Encounter? Encounter { get; set; }
}