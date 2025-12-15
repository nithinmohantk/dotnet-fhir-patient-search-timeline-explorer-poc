using DooLittle.Health.PatientTimeline.Api.Data;
using DooLittle.Health.PatientTimeline.Data;
using DooLittle.Health.PatientTimeline.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Patient Timeline API", Version = "v1" });
});

// Add data layer services
builder.Services.AddDataLayer(builder.Configuration);

builder.Services.AddScoped<DataImportService>();

builder.Services.AddControllers();

builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created and import data (only when not in design-time)
if (Environment.GetEnvironmentVariable("DOTNET_EF_DESIGNTIME") != "1")
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        
        // Check if data already exists
        var patientCount = await context.Patients.CountAsync();
        if (patientCount == 0)
        {
            try
            {
                var importService = scope.ServiceProvider.GetRequiredService<DataImportService>();
                var csvDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data", "csv");
                await importService.ImportSyntheaDataAsync(csvDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                Console.WriteLine("Continuing without data import...");
            }
        }
        else
        {
            Console.WriteLine($"Database already contains {patientCount} patients. Skipping data import.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient Timeline API v1");
        c.RoutePrefix = "docs";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
