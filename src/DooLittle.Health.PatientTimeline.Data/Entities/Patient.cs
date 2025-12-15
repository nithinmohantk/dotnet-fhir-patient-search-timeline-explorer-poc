using System.ComponentModel.DataAnnotations;

namespace DooLittle.Health.PatientTimeline.Data.Entities;

/// <summary>
/// Represents a patient in the healthcare system.
/// Contains comprehensive patient information including demographics, contact details,
/// and healthcare-related financial data.
/// </summary>
public class Patient
{
    /// <summary>
    /// Gets or sets the unique identifier for the patient.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the patient.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the patient.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the medical record number for the patient.
    /// </summary>
    public string? MedicalRecordNumber { get; set; }

    // Synthea-specific fields

    /// <summary>
    /// Gets or sets the Synthea-generated unique identifier for the patient.
    /// </summary>
    public string? SyntheaId { get; set; }

    /// <summary>
    /// Gets or sets the date of death if the patient has passed away.
    /// </summary>
    public DateTime? DeathDate { get; set; }

    /// <summary>
    /// Gets or sets the Social Security Number of the patient.
    /// </summary>
    public string? Ssn { get; set; }

    /// <summary>
    /// Gets or sets the driver's license number of the patient.
    /// </summary>
    public string? DriversLicense { get; set; }

    /// <summary>
    /// Gets or sets the passport number of the patient.
    /// </summary>
    public string? Passport { get; set; }

    /// <summary>
    /// Gets or sets the name prefix (e.g., Mr., Mrs., Dr.).
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Gets or sets the first name of the patient.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the middle name of the patient.
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the patient.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the name suffix (e.g., Jr., Sr., III).
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    /// Gets or sets the maiden name of the patient.
    /// </summary>
    public string? MaidenName { get; set; }

    /// <summary>
    /// Gets or sets the marital status of the patient.
    /// </summary>
    public string? MaritalStatus { get; set; }

    /// <summary>
    /// Gets or sets the race of the patient.
    /// </summary>
    public string? Race { get; set; }

    /// <summary>
    /// Gets or sets the ethnicity of the patient.
    /// </summary>
    public string? Ethnicity { get; set; }

    /// <summary>
    /// Gets or sets the gender of the patient.
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// Gets or sets the birthplace of the patient.
    /// </summary>
    public string? Birthplace { get; set; }

    /// <summary>
    /// Gets or sets the street address of the patient.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the city of residence of the patient.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the state of residence of the patient.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Gets or sets the county of residence of the patient.
    /// </summary>
    public string? County { get; set; }

    /// <summary>
    /// Gets or sets the FIPS code for the patient's location.
    /// </summary>
    public string? Fips { get; set; }

    /// <summary>
    /// Gets or sets the ZIP code of the patient's address.
    /// </summary>
    public string? Zip { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate of the patient's address.
    /// </summary>
    public decimal? Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the patient's address.
    /// </summary>
    public decimal? Lon { get; set; }

    /// <summary>
    /// Gets or sets the total healthcare expenses for the patient.
    /// </summary>
    public decimal? HealthcareExpenses { get; set; }

    /// <summary>
    /// Gets or sets the healthcare coverage amount for the patient.
    /// </summary>
    public decimal? HealthcareCoverage { get; set; }

    /// <summary>
    /// Gets or sets the income of the patient.
    /// </summary>
    public decimal? Income { get; set; }

    // Navigation properties

    /// <summary>
    /// Gets or sets the collection of encounters for this patient.
    /// </summary>
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    /// <summary>
    /// Gets or sets the collection of conditions for this patient.
    /// </summary>
    public ICollection<Condition> Conditions { get; set; } = new List<Condition>();

    /// <summary>
    /// Gets or sets the collection of medications for this patient.
    /// </summary>
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();

    /// <summary>
    /// Gets or sets the collection of procedures for this patient.
    /// </summary>
    public ICollection<Procedure> Procedures { get; set; } = new List<Procedure>();

    /// <summary>
    /// Gets or sets the collection of observations for this patient.
    /// </summary>
    public ICollection<Observation> Observations { get; set; } = new List<Observation>();

    /// <summary>
    /// Gets or sets the collection of immunizations for this patient.
    /// </summary>
    public ICollection<Immunization> Immunizations { get; set; } = new List<Immunization>();
}