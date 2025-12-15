using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Api.Models;

public class Patient
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string? MedicalRecordNumber { get; set; }

    // Synthea-specific fields
    public string? SyntheaId { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Ssn { get; set; }
    public string? DriversLicense { get; set; }
    public string? Passport { get; set; }
    public string? Prefix { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Suffix { get; set; }
    public string? MaidenName { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Race { get; set; }
    public string? Ethnicity { get; set; }
    public string? Gender { get; set; }
    public string? Birthplace { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? County { get; set; }
    public string? Fips { get; set; }
    public string? Zip { get; set; }
    public decimal? Lat { get; set; }
    public decimal? Lon { get; set; }
    public decimal? HealthcareExpenses { get; set; }
    public decimal? HealthcareCoverage { get; set; }
    public decimal? Income { get; set; }

    // Navigation properties
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();
}