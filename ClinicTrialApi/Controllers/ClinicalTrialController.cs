using ClinicTrialApi.Data;
using ClinicTrialApi.Interfaces;
using ClinicTrialApi.Models;
using ClinicTrialApi.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClinicTrialApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalTrialController : ControllerBase
    {
        private readonly IClinicalTrialService _clinicalTrialService;
        private readonly DataContext _context;
        private readonly ILogger<ClinicalTrialService> _logger;


        public ClinicalTrialController(
            IClinicalTrialService clinicalTrialService,
            DataContext context,
            ILogger<ClinicalTrialService> logger)
        {
            _clinicalTrialService = clinicalTrialService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request, CancellationToken token)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            
            var uploadedJson = await _clinicalTrialService.ReadFileContentAsync(file);
            
            if (!_clinicalTrialService.ValidateJson(uploadedJson))
                return BadRequest("Invalid JSON format.");

            var clinicalTrial = JsonConvert.DeserializeObject<ClinicalTrial>(uploadedJson);

            try
            {
                var transformedData = await _clinicalTrialService.TransformDataAsync(clinicalTrial, token);
                return Ok(new { transformedData.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing uploaded file: {ex.Message}");
                return StatusCode(500, $"Error saving into the database: {ex.Message}");
            }
        }

        /*private ClinicalTrial TransformData(ClinicalTrial data)
        {
            // Example business rule: You might want to update or add additional properties here
            data.StartDate = DateTime.Parse(data.StartDate).ToString("yyyy-MM-dd");
            data.EndDate = DateTime.Parse(data.EndDate).ToString("yyyy-MM-dd");

            // You can add other transformation logic here based on your specific requirements

            return data;
        }*/

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinicTrial(int id, CancellationToken token)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be a positive integer.");
            }
            try
            {
                var result = await _clinicalTrialService.GetClinicTrialByIdAsync(id, token);
                return result is not null ? Ok(result) : NotFound();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"Error fetching ClinicalTrial with ID {id}.");
                return StatusCode(500, "An error occurred while fetching the clinical trial.");
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] TrialStatus status, CancellationToken token)
        {
            if (!Enum.IsDefined(typeof(TrialStatus), status))
            {
                return BadRequest($"Invalid status value: {status}. Please provide a valid status.");
            }

            try
            {
                var results = await _clinicalTrialService.FilterByStatusAsync(status, token);
                if (!results.Any())
                {
                    return NotFound($"No clinical trials found for status: {status}");
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error filtering ClinicalTrial data based on status: {status}");
                return StatusCode(500, "An error occurred while filtering the clinical trial.");
            }
        }
    }
}
