using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a timeline event for displaying patient history.
/// This is a derived entity used for timeline visualization.
/// </summary>
public class TimelineEvent
{
    /// <summary>
    /// Gets or sets the unique identifier for the timeline event.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the foreign key to the patient associated with this event.
    /// </summary>
    [Required]
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the patient associated with this event.
    /// </summary>
    [JsonIgnore]
    public Patient? Patient { get; set; }

    /// <summary>
    /// Gets or sets the title of the timeline event.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the timeline event.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the timeline event.
    /// </summary>
    public DateTime EventDate { get; set; }

    /// <summary>
    /// Gets or sets the type of the timeline event (e.g., "Diagnosis", "Treatment", "Appointment").
    /// </summary>
    public string? EventType { get; set; }

    /// <summary>
    /// Gets or sets additional details about the timeline event.
    /// </summary>
    public string? Details { get; set; }
}