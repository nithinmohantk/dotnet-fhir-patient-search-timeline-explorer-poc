using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Api.Models;

public class Organization
{
    public int Id { get; set; }

    [Required]
    public string SyntheaId { get; set; } = string.Empty;

    public string? Name { get; set; }

    // Navigation properties
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}