using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a healthcare organization (hospital, clinic, etc.).
/// Contains organization details and relationships to encounters.
/// </summary>
public class Organization
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Synthea-generated unique identifier for the organization.
    /// </summary>
    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public string? Name { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the collection of encounters that occurred at this organization.
    /// </summary>
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}