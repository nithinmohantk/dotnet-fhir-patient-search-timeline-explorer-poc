using CsvHelper.Configuration.Attributes;

namespace DooLittle.Health.PatientTimeline.Api.Models.Synthea;

public class SyntheaEncounter
{
    [Name("Id")]
    public string Id { get; set; } = string.Empty;

    [Name("START")]
    public DateTime Start { get; set; }

    [Name("STOP")]
    public DateTime? Stop { get; set; }

    [Name("PATIENT")]
    public string PatientId { get; set; } = string.Empty;

    [Name("ORGANIZATION")]
    public string? Organization { get; set; }

    [Name("PROVIDER")]
    public string? Provider { get; set; }

    [Name("PAYER")]
    public string? Payer { get; set; }

    [Name("ENCOUNTERCLASS")]
    public string EncounterClass { get; set; } = string.Empty;

    [Name("CODE")]
    public string Code { get; set; } = string.Empty;

    [Name("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;

    [Name("BASE_ENCOUNTER_COST")]
    public decimal? BaseEncounterCost { get; set; }

    [Name("TOTAL_CLAIM_COST")]
    public decimal? TotalClaimCost { get; set; }

    [Name("PAYER_COVERAGE")]
    public decimal? PayerCoverage { get; set; }

    [Name("REASONCODE")]
    public string? ReasonCode { get; set; }

    [Name("REASONDESCRIPTION")]
    public string? ReasonDescription { get; set; }
}