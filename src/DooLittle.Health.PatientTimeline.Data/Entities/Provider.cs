using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a healthcare provider (doctor, nurse, etc.).
/// Contains provider details and relationships to encounters.
/// </summary>
public class Provider
{
    /// <summary>
    /// Gets or sets the unique identifier for the provider.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Synthea-generated unique identifier for the provider.
    /// </summary>
    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the provider.
    /// </summary>
    public string? Name { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the collection of encounters handled by this provider.
    /// </summary>
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}