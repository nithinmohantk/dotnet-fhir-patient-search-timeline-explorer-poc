using DooLittle.Health.PatientTimeline.Data.Context;
using DooLittle.Health.PatientTimeline.Data.Interfaces;
using DooLittle.Health.PatientTimeline.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DooLittle.Health.PatientTimeline.Data;

/// <summary>
/// Extension methods for configuring data layer services.
/// </summary>
public static class DataServiceCollectionExtensions
{
    /// <summary>
    /// Adds the data layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection with data services added.</returns>
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure database context
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Use PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}