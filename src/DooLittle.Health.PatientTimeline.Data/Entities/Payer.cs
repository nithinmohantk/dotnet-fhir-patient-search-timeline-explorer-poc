using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a healthcare payer (insurance company, government program, etc.).
/// Contains payer details and relationships to encounters and medications.
/// </summary>
public class Payer
{
    /// <summary>
    /// Gets or sets the unique identifier for the payer.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Synthea-generated unique identifier for the payer.
    /// </summary>
    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the payer.
    /// </summary>
    public string? Name { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the collection of encounters covered by this payer.
    /// </summary>
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    /// <summary>
    /// Gets or sets the collection of medications covered by this payer.
    /// </summary>
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
}