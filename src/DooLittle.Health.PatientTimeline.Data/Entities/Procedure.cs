using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a medical procedure performed on a patient.
/// Contains procedure details, timing, costs, and relationships.
/// </summary>
public class Procedure
{
    /// <summary>
    /// Gets or sets the unique identifier for the procedure.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the start date and time of the procedure.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the stop date and time of the procedure (nullable for instant procedures).
    /// </summary>
    public DateTime? Stop { get; set; }

    // Foreign keys

    /// <summary>
    /// Gets or sets the foreign key to the patient who underwent this procedure.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the encounter during which this procedure was performed.
    /// </summary>
    public int? EncounterId { get; set; }

    // Procedure details

    /// <summary>
    /// Gets or sets the coding system used for the procedure code.
    /// </summary>
    public string? System { get; set; }

    /// <summary>
    /// Gets or sets the procedure code from the coding system.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the procedure.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the base cost of the procedure.
    /// </summary>
    public decimal? BaseCost { get; set; }

    /// <summary>
    /// Gets or sets the reason code for performing this procedure.
    /// </summary>
    public string? ReasonCode { get; set; }

    /// <summary>
    /// Gets or sets the description of the reason for performing this procedure.
    /// </summary>
    public string? ReasonDescription { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the patient who underwent this procedure.
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Gets or sets the encounter during which this procedure was performed.
    /// </summary>
    public Encounter? Encounter { get; set; }
}