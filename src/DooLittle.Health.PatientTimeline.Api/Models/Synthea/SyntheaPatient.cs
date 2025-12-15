using CsvHelper.Configuration.Attributes;

namespace DooLittle.Health.PatientTimeline.Api.Models.Synthea;

public class SyntheaPatient
{
    [Name("Id")]
    public string Id { get; set; } = string.Empty;

    [Name("BIRTHDATE")]
    public DateTime BirthDate { get; set; }

    [Name("DEATHDATE")]
    public DateTime? DeathDate { get; set; }

    [Name("SSN")]
    public string? Ssn { get; set; }

    [Name("PREFIX")]
    public string? Prefix { get; set; }

    [Name("FIRST")]
    public string FirstName { get; set; } = string.Empty;

    [Name("LAST")]
    public string LastName { get; set; } = string.Empty;

    [Name("SUFFIX")]
    public string? Suffix { get; set; }

    [Name("MAIDEN")]
    public string? MaidenName { get; set; }

    [Name("MARITAL")]
    public string? MaritalStatus { get; set; }

    [Name("RACE")]
    public string? Race { get; set; }

    [Name("ETHNICITY")]
    public string? Ethnicity { get; set; }

    [Name("GENDER")]
    public string Gender { get; set; } = string.Empty;

    [Name("BIRTHPLACE")]
    public string? BirthPlace { get; set; }

    [Name("ADDRESS")]
    public string? Address { get; set; }

    [Name("CITY")]
    public string? City { get; set; }

    [Name("STATE")]
    public string? State { get; set; }

    [Name("COUNTY")]
    public string? County { get; set; }

    [Name("ZIP")]
    public string? Zip { get; set; }

    [Name("LAT")]
    public double? Latitude { get; set; }

    [Name("LON")]
    public double? Longitude { get; set; }
}