using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Api.Models;

public class Encounter
{
    public int Id { get; set; }

    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    public DateTime Start { get; set; }
    public DateTime? Stop { get; set; }

    // Foreign keys
    public int PatientId { get; set; }
    public int? OrganizationId { get; set; }
    public int? ProviderId { get; set; }
    public int? PayerId { get; set; }

    // Encounter details
    public string? EncounterClass { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public decimal? BaseEncounterCost { get; set; }
    public decimal? TotalClaimCost { get; set; }
    public decimal? PayerCoverage { get; set; }
    public string? ReasonCode { get; set; }
    public string? ReasonDescription { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Organization? Organization { get; set; }
    public Provider? Provider { get; set; }
    public Payer? Payer { get; set; }
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
}