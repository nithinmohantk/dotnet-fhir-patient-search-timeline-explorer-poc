using DooLittle.Health.PatientTimeline.Api.Data;
using DooLittle.Health.PatientTimeline.Data.Entities;
using DooLittle.Health.PatientTimeline.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DooLittle.Health.PatientTimeline.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DataImportService _dataImportService;

    public PatientsController(IUnitOfWork unitOfWork, DataImportService dataImportService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _dataImportService = dataImportService ?? throw new ArgumentNullException(nameof(dataImportService));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetPatients()
    {
        var patients = await _unitOfWork.Patients.GetAllAsync();
        return patients.Select(p => new
        {
            p.Id,
            p.FirstName,
            p.LastName,
            p.DateOfBirth,
            p.Gender,
            p.SyntheaId
        }).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpGet("by-synthea/{syntheaId}")]
    public async Task<ActionResult<Patient>> GetPatientBySyntheaId(string syntheaId)
    {
        var patient = await _unitOfWork.Patients.GetBySyntheaIdAsync(syntheaId);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpGet("by-synthea/{syntheaId}/timeline")]
    public async Task<ActionResult<object>> GetPatientTimeline(string syntheaId)
    {
        var patient = await _unitOfWork.Patients.GetBySyntheaIdAsync(syntheaId);

        if (patient == null)
        {
            return NotFound();
        }

        // Construct FHIR filename: FirstName_LastName_SyntheaId.json
        var fileName = $"{patient.FirstName}_{patient.LastName}_{patient.SyntheaId}.json";
        var fhirDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data", "fhir");
        var filePath = Path.Combine(fhirDirectory, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"FHIR file not found: {fileName}");
        }

        try
        {
            var fhirContent = await System.IO.File.ReadAllTextAsync(filePath);
            return Content(fhirContent, "application/json");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error reading FHIR file: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> PostPatient(Patient patient)
    {
        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(int id, Patient patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        await _unitOfWork.Patients.UpdateAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var deleted = await _unitOfWork.Patients.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportCsvData()
    {
        try
        {
            var csvDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "csv");
            await _dataImportService.ImportSyntheaDataAsync(csvDirectory);
            return Ok("CSV data imported successfully");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error importing CSV data: {ex.Message}");
        }
    }
}