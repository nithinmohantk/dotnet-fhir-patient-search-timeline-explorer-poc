using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DooLittle.Health.PatientTimeline.Api.Models;

public class TimelineEvent
{
    public int Id { get; set; }

    [Required]
    public int PatientId { get; set; }

    [JsonIgnore]
    public Patient? Patient { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public string? EventType { get; set; } // e.g., "Diagnosis", "Treatment", "Appointment"

    public string? Details { get; set; }
}