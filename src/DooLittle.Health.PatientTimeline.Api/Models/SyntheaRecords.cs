using CsvHelper.Configuration.Attributes;

namespace backend.Models;

public class SyntheaPatient
{
    [Name("Id")]
    public string Id { get; set; } = string.Empty;

    [Name("BIRTHDATE")]
    public DateTime BirthDate { get; set; }

    [Name("DEATHDATE")]
    public DateTime? DeathDate { get; set; }

    [Name("FIRST")]
    public string FirstName { get; set; } = string.Empty;

    [Name("LAST")]
    public string LastName { get; set; } = string.Empty;

    [Name("GENDER")]
    public string Gender { get; set; } = string.Empty;

    [Name("RACE")]
    public string Race { get; set; } = string.Empty;

    [Name("ETHNICITY")]
    public string Ethnicity { get; set; } = string.Empty;
}

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

    [Name("ENCOUNTERCLASS")]
    public string EncounterClass { get; set; } = string.Empty;

    [Name("CODE")]
    public string Code { get; set; } = string.Empty;

    [Name("DESCRIPTION")]
    public string Description { get; set; } = string.Empty;

    [Name("REASONDESCRIPTION")]
    public string? ReasonDescription { get; set; }
}

public class SyntheaCondition
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
}

public class SyntheaMedication
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

    [Name("REASONDESCRIPTION")]
    public string? ReasonDescription { get; set; }
}

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

    [Name("REASONDESCRIPTION")]
    public string? ReasonDescription { get; set; }
}

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
    public string Value { get; set; } = string.Empty;

    [Name("UNITS")]
    public string? Units { get; set; }
}

public class SyntheaImmunization
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
}