using CsvHelper.Configuration.Attributes;

namespace DooLittle.Health.PatientTimeline.Api.Models.Synthea;

public class SyntheaProcedure
{
    [Name("START")]
    public DateTime Start { get; set; }

    [Name("STOP")]
    public DateTime? Stop { get; set; }

    [Name("PATIENT")]
    public string PatientId { get; set; } = string.Empty;

    [Name("ENCOUNTER")]
    public string EncounterId { get; set; } = string.Empty;

    [Name("CODE")]
    public string Code { get; set; } = string.Empty;

    [Name("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;

    [Name("BASE_COST")]
    public decimal? BaseCost { get; set; }

    [Name("PAYER_COVERAGE")]
    public decimal? PayerCoverage { get; set; }

    [Name("REASONCODE")]
    public string? ReasonCode { get; set; }

    [Name("REASONDESCRIPTION")]
    public string? ReasonDescription { get; set; }
}