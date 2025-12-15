using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents an immunization or vaccination given to a patient.
/// Contains immunization details and relationships to patients and encounters.
/// </summary>
public class Immunization
{
    /// <summary>
    /// Gets or sets the unique identifier for the immunization.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date of the immunization.
    /// </summary>
    public DateTime Date { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient who received this immunization.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the encounter where this immunization was given.
    /// </summary>
    public int? EncounterId { get; set; }

    // Immunization details

    /// <summary>
    /// Gets or sets the coding system used for the immunization code.
    /// </summary>
    public string? System { get; set; }

    /// <summary>
    /// Gets or sets the immunization code from the coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the immunization.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the base cost of the immunization.
    /// </summary>
    public decimal? BaseCost { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient who received this immunization.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the encounter where this immunization was given.
    /// </summary>
    public Encounter? Encounter { get; set; }
}