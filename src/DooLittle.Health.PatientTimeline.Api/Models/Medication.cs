using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Api.Models;

public class Medication
{
    public int Id { get; set; }

    public DateTime Start { get; set; }
    public DateTime? Stop { get; set; }

    // Foreign keys
    public int PatientId { get; set; }
    public int? EncounterId { get; set; }
    public int? PayerId { get; set; }

    // Medication details
    public string? Code { get; set; }
    public string? Description { get; set; }
    public decimal? BaseCost { get; set; }
    public decimal? PayerCoverage { get; set; }
    public int? Dispenses { get; set; }
    public decimal? TotalCost { get; set; }
    public string? ReasonCode { get; set; }
    public string? ReasonDescription { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Encounter? Encounter { get; set; }
    public Payer? Payer { get; set; }
}