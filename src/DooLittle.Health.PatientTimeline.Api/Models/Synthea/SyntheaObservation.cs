using CsvHelper.Configuration.Attributes;

namespace DooLittle.Health.PatientTimeline.Api.Models.Synthea;

public class SyntheaObservation
{
    [Name("DATE")]
    public DateTime Date { get; set; }

    [Name("PATIENT")]
    public string PatientId { get; set; } = string.Empty;

    [Name("ENCOUNTER")]
    public string EncounterId { get; set; } = string.Empty;

    [Name("CODE")]
    public string Code { get; set; } = string.Empty;

    [Name("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;

    [Name("VALUE")]
    public string? Value { get; set; }

    [Name("UNITS")]
    public string? Units { get; set; }
}