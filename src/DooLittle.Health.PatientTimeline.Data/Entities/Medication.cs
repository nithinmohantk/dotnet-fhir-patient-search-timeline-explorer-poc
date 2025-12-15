using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a medication prescribed to a patient.
/// Contains medication details, costs, dispensing information, and relationships.
/// </summary>
public class Medication
{
    /// <summary>
    /// Gets or sets the unique identifier for the medication.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the start date of the medication prescription.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the stop date of the medication prescription (nullable if ongoing).
    /// </summary>
    public DateTime? Stop { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient who was prescribed this medication.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the encounter where this medication was prescribed.
    /// </summary>
    public int? EncounterId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the payer responsible for this medication.
    /// </summary>
    public int? PayerId { get; set; }

    // Medication details

    /// <summary>
    /// Gets or sets the medication code from a coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the medication.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the base cost of the medication.
    /// </summary>
    public decimal? BaseCost { get; set; }

    /// <summary>
    /// Gets or sets the amount covered by the payer for this medication.
    /// </summary>
    public decimal? PayerCoverage { get; set; }

    /// <summary>
    /// Gets or sets the number of dispenses for this medication.
    /// </summary>
    public int? Dispenses { get; set; }

    /// <summary>
    /// Gets or sets the total cost of the medication including all dispenses.
    /// </summary>
    public decimal? TotalCost { get; set; }

    /// <summary>
    /// Gets or sets the reason code for prescribing this medication.
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// Gets or sets the description of the reason for prescribing this medication.
    /// </summary>
    public string? ReasonDescription { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient who was prescribed this medication.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the encounter where this medication was prescribed.
    /// </summary>
    public Encounter? Encounter { get; set; }

    /// <summary>
    /// Gets or sets the payer responsible for this medication.
    /// </summary>
    public Payer? Payer { get; set; }
}