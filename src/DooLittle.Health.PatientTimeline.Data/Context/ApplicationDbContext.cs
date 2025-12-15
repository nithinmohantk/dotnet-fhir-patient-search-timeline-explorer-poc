using Microsoft.EntityFrameworkCore;
using DooLittle.Health.PatientTimeline.Data.Entities;

namespace DooLittle.Health.PatientTimeline.Data.Context;

/// <summary>
/// Application database context for the Patient Timeline system.
/// Manages all entity configurations and relationships for healthcare data.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the patients DbSet.
    /// </summary>
    public DbSet<Patient> Patients { get; set; }

    /// <summary>
    /// Gets or sets the encounters DbSet.
    /// </summary>
    public DbSet<Encounter> Encounters { get; set; }

    /// <summary>
    /// Gets or sets the conditions DbSet.
    /// </summary>
    public DbSet<Condition> Conditions { get; set; }

    /// <summary>
    /// Gets or sets the medications DbSet.
    /// </summary>
    public DbSet<Medication> Medications { get; set; }

    /// <summary>
    /// Gets or sets the procedures DbSet.
    /// </summary>
    public DbSet<Procedure> Procedures { get; set; }

    /// <summary>
    /// Gets or sets the observations DbSet.
    /// </summary>
    public DbSet<Observation> Observations { get; set; }

    /// <summary>
    /// Gets or sets the immunizations DbSet.
    /// </summary>
    public DbSet<Immunization> Immunizations { get; set; }

    /// <summary>
    /// Gets or sets the organizations DbSet.
    /// </summary>
    public DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Gets or sets the providers DbSet.
    /// </summary>
    public DbSet<Provider> Providers { get; set; }

    /// <summary>
    /// Gets or sets the payers DbSet.
    /// </summary>
    public DbSet<Payer> Payers { get; set; }

    /// <summary>
    /// Gets or sets the timeline events DbSet.
    /// </summary>
    public DbSet<TimelineEvent> TimelineEvents { get; set; }

    /// <summary>
    /// Configures the model relationships and constraints.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Patient relationships
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Encounters)
            .WithOne(e => e.Patient)
            .HasForeignKey(e => e.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Conditions)
            .WithOne(c => c.Patient)
            .HasForeignKey(c => c.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Medications)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Procedures)
            .WithOne(p => p.Patient)
            .HasForeignKey(p => p.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Observations)
            .WithOne(o => o.Patient)
            .HasForeignKey(o => o.PatientId);

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Immunizations)
            .WithOne(i => i.Patient)
            .HasForeignKey(i => i.PatientId);

        // Encounter relationships
        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.Organization)
            .WithMany(o => o.Encounters)
            .HasForeignKey(e => e.OrganizationId);

        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.Provider)
            .WithMany(p => p.Encounters)
            .HasForeignKey(e => e.ProviderId);

        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.Payer)
            .WithMany(p => p.Encounters)
            .HasForeignKey(e => e.PayerId);

        modelBuilder.Entity<Encounter>()
            .HasMany(e => e.Conditions)
            .WithOne(c => c.Encounter)
            .HasForeignKey(c => c.EncounterId);

        modelBuilder.Entity<Encounter>()
            .HasMany(e => e.Medications)
            .WithOne(m => m.Encounter)
            .HasForeignKey(m => m.EncounterId);

        modelBuilder.Entity<Encounter>()
            .HasMany(e => e.Procedures)
            .WithOne(p => p.Encounter)
            .HasForeignKey(p => p.EncounterId);

        // Medication relationships
        modelBuilder.Entity<Medication>()
            .HasOne(m => m.Payer)
            .WithMany(p => p.Medications)
            .HasForeignKey(m => m.PayerId);

        // Configure decimal precision for monetary values
        modelBuilder.Entity<Patient>()
            .Property(p => p.HealthcareExpenses)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Patient>()
            .Property(p => p.HealthcareCoverage)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Patient>()
            .Property(p => p.Income)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Encounter>()
            .Property(e => e.BaseEncounterCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Encounter>()
            .Property(e => e.TotalClaimCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Encounter>()
            .Property(e => e.PayerCoverage)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Medication>()
            .Property(m => m.BaseCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Medication>()
            .Property(m => m.PayerCoverage)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Medication>()
            .Property(m => m.TotalCost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Procedure>()
            .Property(p => p.BaseCost)
            .HasPrecision(18, 2);

        // Configure latitude/longitude precision
        modelBuilder.Entity<Patient>()
            .Property(p => p.Lat)
            .HasPrecision(10, 7);

        modelBuilder.Entity<Patient>()
            .Property(p => p.Lon)
            .HasPrecision(10, 7);

        // Configure DateTime properties to use UTC
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }
        }
    }
}