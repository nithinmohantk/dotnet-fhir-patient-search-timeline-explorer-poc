using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a healthcare encounter (visit, admission, etc.) for a patient.
/// Contains encounter details, costs, and relationships to healthcare providers and organizations.
/// </summary>
public class Encounter
{
    /// <summary>
    /// Gets or sets the unique identifier for the encounter.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Synthea-generated unique identifier for the encounter.
    /// </summary>
    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date and time of the encounter.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the stop date and time of the encounter (nullable for ongoing encounters).
    /// </summary>
    public DateTime? Stop { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient involved in this encounter.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the organization where the encounter occurred.
    /// </summary>
    public int? OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the provider who handled the encounter.
    /// </summary>
    public int? ProviderId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the payer responsible for the encounter costs.
    /// </summary>
    public int? PayerId { get; set; }

    // Encounter details

    /// <summary>
    /// Gets or sets the class/type of the encounter (e.g., inpatient, outpatient, emergency).
    /// </summary>
    public string? EncounterClass { get; set; }

    /// <summary>
    /// Gets or sets the encounter code from a coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the encounter.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the base cost of the encounter.
    /// </summary>
    public decimal? BaseEncounterCost { get; set; }

    /// <summary>
    /// Gets or sets the total claim cost for the encounter.
    /// </summary>
    public decimal? TotalClaimCost { get; set; }

    /// <summary>
    /// Gets or sets the amount covered by the payer.
    /// </summary>
    public decimal? PayerCoverage { get; set; }

    /// <summary>
    /// Gets or sets the reason code for the encounter.
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// Gets or sets the description of the reason for the encounter.
    /// </summary>
    public string? ReasonDescription { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient associated with this encounter.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the organization where the encounter occurred.
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// Gets or sets the provider who handled the encounter.
    /// </summary>
    public Provider? Provider { get; set; }

    /// <summary>
    /// Gets or sets the payer responsible for the encounter costs.
    /// </summary>
    public Payer? Payer { get; set; }

    /// <summary>
    /// Gets or sets the collection of conditions diagnosed during this encounter.
    /// </summary>
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();

    /// <summary>
    /// Gets or sets the collection of medications prescribed during this encounter.
    /// </summary>
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();

    /// <summary>
    /// Gets or sets the collection of procedures performed during this encounter.
    /// </summary>
    public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
}