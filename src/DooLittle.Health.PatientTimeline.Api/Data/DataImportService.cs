using DooLittle.Health.PatientTimeline.Data.Context;
using DooLittle.Health.PatientTimeline.Data.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace DooLittle.Health.PatientTimeline.Api.Data;

public class DataImportService
{
    private readonly ApplicationDbContext _context;

    public DataImportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ImportSyntheaDataAsync(string csvDirectory)
    {
        Console.WriteLine("Starting Synthea data import...");

        // Import organizations, providers, and payers first (referenced by encounters)
        await ImportOrganizationsAsync(Path.Combine(csvDirectory, "organizations.csv"));
        await ImportProvidersAsync(Path.Combine(csvDirectory, "providers.csv"));
        await ImportPayersAsync(Path.Combine(csvDirectory, "payers.csv"));

        // Import patients
        await ImportPatientsAsync(Path.Combine(csvDirectory, "patients.csv"));

        // Import encounters (references patients, organizations, providers, payers)
        await ImportEncountersAsync(Path.Combine(csvDirectory, "encounters.csv"));

        // Import conditions, medications, procedures (reference patients and encounters)
        try
        {
            await ImportConditionsAsync(Path.Combine(csvDirectory, "conditions.csv"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing conditions: {ex.Message}");
        }

        try
        {
            await ImportMedicationsAsync(Path.Combine(csvDirectory, "medications.csv"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing medications: {ex.Message}");
        }

        await ImportProceduresAsync(Path.Combine(csvDirectory, "procedures.csv"));

        try
        {
            await ImportObservationsAsync(Path.Combine(csvDirectory, "observations.csv"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing observations: {ex.Message}");
        }

        try
        {
            await ImportImmunizationsAsync(Path.Combine(csvDirectory, "immunizations.csv"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing immunizations: {ex.Message}");
        }

        Console.WriteLine("Synthea data import completed.");
    }

    private async Task ImportOrganizationsAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Organizations file not found: {filePath}");
            return;
        }

        Console.WriteLine("Importing organizations...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var organizations = new List<Organization>();
        var existingOrgIds = _context.Organizations.Select(o => o.SyntheaId).ToHashSet();

        foreach (var record in records)
        {
            var syntheaId = record.Id?.ToString();
            if (string.IsNullOrEmpty(syntheaId) || existingOrgIds.Contains(syntheaId)) continue;

            organizations.Add(new Organization
            {
                SyntheaId = syntheaId,
                Name = record.NAME?.ToString()
            });
        }

        if (organizations.Any())
        {
            _context.Organizations.AddRange(organizations);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {organizations.Count} organizations.");
        }
    }

    private async Task ImportProvidersAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Providers file not found: {filePath}");
            return;
        }

        Console.WriteLine("Importing providers...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var providers = new List<Provider>();
        var existingProviderIds = _context.Providers.Select(p => p.SyntheaId).ToHashSet();

        foreach (var record in records)
        {
            var syntheaId = record.Id?.ToString();
            if (string.IsNullOrEmpty(syntheaId) || existingProviderIds.Contains(syntheaId)) continue;

            providers.Add(new Provider
            {
                SyntheaId = syntheaId,
                Name = record.NAME?.ToString()
            });
        }

        if (providers.Any())
        {
            _context.Providers.AddRange(providers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {providers.Count} providers.");
        }
    }

    private async Task ImportPayersAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Payers file not found: {filePath}");
            return;
        }

        Console.WriteLine("Importing payers...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var payers = new List<Payer>();
        var existingPayerIds = _context.Payers.Select(p => p.SyntheaId).ToHashSet();

        foreach (var record in records)
        {
            var syntheaId = record.Id?.ToString();
            if (string.IsNullOrEmpty(syntheaId) || existingPayerIds.Contains(syntheaId)) continue;

            payers.Add(new Payer
            {
                SyntheaId = syntheaId,
                Name = record.NAME?.ToString()
            });
        }

        if (payers.Any())
        {
            _context.Payers.AddRange(payers);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {payers.Count} payers.");
        }
    }

    private async Task ImportPatientsAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        Console.WriteLine("Importing patients...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var patients = new List<Patient>();
        var existingPatientIds = _context.Patients.Select(p => p.SyntheaId).ToHashSet();

        foreach (var record in records)
        {
            var syntheaId = record.Id?.ToString();
            if (string.IsNullOrEmpty(syntheaId) || existingPatientIds.Contains(syntheaId)) continue;

            var patient = new Patient
            {
                SyntheaId = syntheaId,
                Name = $"{record.FIRST} {record.LAST}".Trim(),
                FirstName = record.FIRST?.ToString(),
                MiddleName = record.MIDDLE?.ToString(),
                LastName = record.LAST?.ToString(),
                MedicalRecordNumber = syntheaId,
                DateOfBirth = DateTime.SpecifyKind(DateTime.Parse(record.BIRTHDATE?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                DeathDate = string.IsNullOrEmpty(record.DEATHDATE?.ToString()) ? null : DateTime.SpecifyKind(DateTime.Parse(record.DEATHDATE.ToString()), DateTimeKind.Utc),
                Ssn = record.SSN?.ToString(),
                DriversLicense = record.DRIVERS?.ToString(),
                Passport = record.PASSPORT?.ToString(),
                Prefix = record.PREFIX?.ToString(),
                Suffix = record.SUFFIX?.ToString(),
                MaidenName = record.MAIDEN?.ToString(),
                MaritalStatus = record.MARITAL?.ToString(),
                Race = record.RACE?.ToString(),
                Ethnicity = record.ETHNICITY?.ToString(),
                Gender = record.GENDER?.ToString(),
                Birthplace = record.BIRTHPLACE?.ToString(),
                Address = record.ADDRESS?.ToString(),
                City = record.CITY?.ToString(),
                State = record.STATE?.ToString(),
                County = record.COUNTY?.ToString(),
                Fips = record.FIPS?.ToString(),
                Zip = record.ZIP?.ToString(),
                Lat = string.IsNullOrEmpty(record.LAT?.ToString()) ? null : decimal.Parse(record.LAT.ToString()),
                Lon = string.IsNullOrEmpty(record.LON?.ToString()) ? null : decimal.Parse(record.LON.ToString()),
                HealthcareExpenses = string.IsNullOrEmpty(record.HEALTHCARE_EXPENSES?.ToString()) ? null : decimal.Parse(record.HEALTHCARE_EXPENSES.ToString()),
                HealthcareCoverage = string.IsNullOrEmpty(record.HEALTHCARE_COVERAGE?.ToString()) ? null : decimal.Parse(record.HEALTHCARE_COVERAGE.ToString()),
                Income = string.IsNullOrEmpty(record.INCOME?.ToString()) ? null : decimal.Parse(record.INCOME.ToString())
            };
            patients.Add(patient);
        }

        if (patients.Any())
        {
            _context.Patients.AddRange(patients);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {patients.Count} patients.");
        }
    }

    private async Task ImportEncountersAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        Console.WriteLine("Importing encounters...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var encounters = new List<Encounter>();
        var existingEncounterIds = _context.Encounters.Select(e => e.SyntheaId).ToHashSet();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var orgLookup = _context.Organizations.ToDictionary(o => o.SyntheaId, o => o.Id);
        var providerLookup = _context.Providers.ToDictionary(p => p.SyntheaId, p => p.Id);
        var payerLookup = _context.Payers.ToDictionary(p => p.SyntheaId, p => p.Id);

        foreach (var record in records)
        {
            var syntheaId = record.Id?.ToString();
            if (string.IsNullOrEmpty(syntheaId) || existingEncounterIds.Contains(syntheaId)) continue;

            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var encounter = new Encounter
            {
                SyntheaId = syntheaId,
                Start = DateTime.SpecifyKind(DateTime.Parse(record.START?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                Stop = string.IsNullOrEmpty(record.STOP?.ToString()) ? null : DateTime.SpecifyKind(DateTime.Parse(record.STOP.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                EncounterClass = record.ENCOUNTERCLASS?.ToString(),
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString(),
                BaseEncounterCost = string.IsNullOrEmpty(record.BASE_ENCOUNTER_COST?.ToString()) ? null : decimal.Parse(record.BASE_ENCOUNTER_COST.ToString()),
                TotalClaimCost = string.IsNullOrEmpty(record.TOTAL_CLAIM_COST?.ToString()) ? null : decimal.Parse(record.TOTAL_CLAIM_COST.ToString()),
                PayerCoverage = string.IsNullOrEmpty(record.PAYER_COVERAGE?.ToString()) ? null : decimal.Parse(record.PAYER_COVERAGE.ToString()),
                ReasonCode = record.REASONCODE?.ToString(),
                ReasonDescription = record.REASONDESCRIPTION?.ToString()
            };

            // Set foreign keys for organization, provider, payer
            var orgSyntheaId = record.ORGANIZATION?.ToString();
            int orgId = 0;
            if (!string.IsNullOrEmpty(orgSyntheaId) && orgLookup.TryGetValue(orgSyntheaId, out orgId))
            {
                encounter.OrganizationId = orgId;
            }

            var providerSyntheaId = record.PROVIDER?.ToString();
            int providerId = 0;
            if (!string.IsNullOrEmpty(providerSyntheaId) && providerLookup.TryGetValue(providerSyntheaId, out providerId))
            {
                encounter.ProviderId = providerId;
            }

            var payerSyntheaId = record.PAYER?.ToString();
            int payerId = 0;
            if (!string.IsNullOrEmpty(payerSyntheaId) && payerLookup.TryGetValue(payerSyntheaId, out payerId))
            {
                encounter.PayerId = payerId;
            }

            encounters.Add(encounter);
        }

        if (encounters.Any())
        {
            _context.Encounters.AddRange(encounters);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {encounters.Count} encounters.");
        }
    }

    private async Task ImportConditionsAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        Console.WriteLine("Importing conditions...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var conditions = new List<Condition>();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var encounterLookup = _context.Encounters.ToDictionary(e => e.SyntheaId, e => e.Id);

        foreach (var record in records)
        {
            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var condition = new Condition
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(record.START?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                Stop = string.IsNullOrEmpty(record.STOP?.ToString()) ? null : DateTime.SpecifyKind(DateTime.Parse(record.STOP.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                System = record.SYSTEM?.ToString(),
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString()
            };

            // Set encounter foreign key if encounter exists
            var encounterSyntheaId = record.ENCOUNTER?.ToString();
            int encounterId = 0;
            if (!string.IsNullOrEmpty(encounterSyntheaId) && encounterLookup.TryGetValue(encounterSyntheaId, out encounterId))
            {
                condition.EncounterId = encounterId;
            }

            conditions.Add(condition);
        }

        if (conditions.Any())
        {
            _context.Conditions.AddRange(conditions);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {conditions.Count} conditions.");
        }
    }

    private async Task ImportMedicationsAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        Console.WriteLine("Importing medications...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var medications = new List<Medication>();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var encounterLookup = _context.Encounters.ToDictionary(e => e.SyntheaId, e => e.Id);
        var payerLookup = _context.Payers.ToDictionary(p => p.SyntheaId, p => p.Id);

        foreach (var record in records)
        {
            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var medication = new Medication
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(record.START?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                Stop = string.IsNullOrEmpty(record.STOP?.ToString()) ? null : DateTime.SpecifyKind(DateTime.Parse(record.STOP.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString(),
                BaseCost = string.IsNullOrEmpty(record.BASE_COST?.ToString()) ? null : decimal.Parse(record.BASE_COST.ToString()),
                PayerCoverage = string.IsNullOrEmpty(record.PAYER_COVERAGE?.ToString()) ? null : decimal.Parse(record.PAYER_COVERAGE.ToString()),
                Dispenses = string.IsNullOrEmpty(record.DISPENSES?.ToString()) ? null : int.Parse(record.DISPENSES.ToString()),
                TotalCost = string.IsNullOrEmpty(record.TOTALCOST?.ToString()) ? null : decimal.Parse(record.TOTALCOST.ToString()),
                ReasonCode = record.REASONCODE?.ToString(),
                ReasonDescription = record.REASONDESCRIPTION?.ToString()
            };

            // Set encounter foreign key if encounter exists
            var encounterSyntheaId = record.ENCOUNTER?.ToString();
            int encounterId = 0;
            if (!string.IsNullOrEmpty(encounterSyntheaId) && encounterLookup.TryGetValue(encounterSyntheaId, out encounterId))
            {
                medication.EncounterId = encounterId;
            }

            // Set payer foreign key if payer exists
            var payerSyntheaId = record.PAYER?.ToString();
            int payerId = 0;
            if (!string.IsNullOrEmpty(payerSyntheaId) && payerLookup.TryGetValue(payerSyntheaId, out payerId))
            {
                medication.PayerId = payerId;
            }

            medications.Add(medication);
        }

        if (medications.Any())
        {
            _context.Medications.AddRange(medications);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {medications.Count} medications.");
        }
    }

    private async Task ImportProceduresAsync(string filePath)
    {
        if (!File.Exists(filePath)) return;

        Console.WriteLine("Importing procedures...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var procedures = new List<Procedure>();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var encounterLookup = _context.Encounters.ToDictionary(e => e.SyntheaId, e => e.Id);

        foreach (var record in records)
        {
            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var procedure = new Procedure
            {
                Start = DateTime.SpecifyKind(DateTime.Parse(record.START?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                Stop = string.IsNullOrEmpty(record.STOP?.ToString()) ? null : DateTime.SpecifyKind(DateTime.Parse(record.STOP.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                System = record.SYSTEM?.ToString(),
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString(),
                BaseCost = string.IsNullOrEmpty(record.BASE_COST?.ToString()) ? null : decimal.Parse(record.BASE_COST.ToString()),
                ReasonCode = record.REASONCODE?.ToString(),
                ReasonDescription = record.REASONDESCRIPTION?.ToString()
            };

            // Set encounter foreign key if encounter exists
            var encounterSyntheaId = record.ENCOUNTER?.ToString();
            int encounterId = 0;
            if (!string.IsNullOrEmpty(encounterSyntheaId) && encounterLookup.TryGetValue(encounterSyntheaId, out encounterId))
            {
                procedure.EncounterId = encounterId;
            }

            procedures.Add(procedure);
        }

        if (procedures.Any())
        {
            _context.Procedures.AddRange(procedures);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {procedures.Count} procedures.");
        }
    }

    private async Task ImportObservationsAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Observations file not found: {filePath}");
            return;
        }

        Console.WriteLine("Importing observations...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var observations = new List<Observation>();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var encounterLookup = _context.Encounters.ToDictionary(e => e.SyntheaId, e => e.Id);

        foreach (var record in records)
        {
            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var observation = new Observation
            {
                Date = DateTime.SpecifyKind(DateTime.Parse(record.DATE?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                System = record.SYSTEM?.ToString(),
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString(),
                Value = record.VALUE?.ToString(),
                Units = record.UNITS?.ToString(),
                Type = record.TYPE?.ToString()
            };

            // Set encounter foreign key if encounter exists
            var encounterSyntheaId = record.ENCOUNTER?.ToString();
            int encounterId = 0;
            if (!string.IsNullOrEmpty(encounterSyntheaId) && encounterLookup.TryGetValue(encounterSyntheaId, out encounterId))
            {
                observation.EncounterId = encounterId;
            }

            observations.Add(observation);
        }

        if (observations.Any())
        {
            _context.Observations.AddRange(observations);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {observations.Count} observations.");
        }
    }

    private async Task ImportImmunizationsAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Immunizations file not found: {filePath}");
            return;
        }

        Console.WriteLine("Importing immunizations...");
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null
        });

        var records = csv.GetRecords<dynamic>().ToList();
        var immunizations = new List<Immunization>();
        var patientLookup = _context.Patients
            .Where(p => p.SyntheaId != null)
            .ToDictionary(p => p.SyntheaId!, p => p.Id);
        var encounterLookup = _context.Encounters.ToDictionary(e => e.SyntheaId, e => e.Id);

        foreach (var record in records)
        {
            var patientSyntheaId = record.PATIENT?.ToString();
            int patientId = 0;
            if (string.IsNullOrEmpty(patientSyntheaId) || !patientLookup.TryGetValue(patientSyntheaId, out patientId)) continue;

            var immunization = new Immunization
            {
                Date = DateTime.SpecifyKind(DateTime.Parse(record.DATE?.ToString() ?? DateTime.MinValue.ToString()), DateTimeKind.Utc),
                PatientId = patientId,
                System = record.SYSTEM?.ToString(),
                Code = record.CODE?.ToString(),
                Description = record.DESCRIPTION?.ToString(),
                BaseCost = string.IsNullOrEmpty(record.BASE_COST?.ToString()) ? null : decimal.Parse(record.BASE_COST.ToString())
            };

            // Set encounter foreign key if encounter exists
            var encounterSyntheaId = record.ENCOUNTER?.ToString();
            int encounterId = 0;
            if (!string.IsNullOrEmpty(encounterSyntheaId) && encounterLookup.TryGetValue(encounterSyntheaId, out encounterId))
            {
                immunization.EncounterId = encounterId;
            }

            immunizations.Add(immunization);
        }

        if (immunizations.Any())
        {
            _context.Immunizations.AddRange(immunizations);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Imported {immunizations.Count} immunizations.");
        }
    }
}