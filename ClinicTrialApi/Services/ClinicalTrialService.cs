using ClinicTrialApi.Data;
using ClinicTrialApi.Interfaces;
using ClinicTrialApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace ClinicTrialApi.Services
{
    public class ClinicalTrialService : IClinicalTrialService
    {
        private readonly DataContext _context;
        private readonly ILogger<ClinicalTrialService> _logger;
        private readonly IResourceHelper _resourceHelper;

        public ClinicalTrialService(
            DataContext context,
            ILogger<ClinicalTrialService> logger,
            IResourceHelper resourceHelper)
        {
            _context = context;
            _logger = logger;
            _resourceHelper = resourceHelper;
        }

        public bool ValidateJson(string uploadedJson)
        {
            var validationSchema = _resourceHelper.GetEmbeddedResource("ClinicalTrialSchema.json");
            var schema = JSchema.Parse(validationSchema);

            var uploadSchema = JObject.Parse(uploadedJson);
            bool isFileValid = uploadSchema.IsValid(schema, out IList<string> validationErrors);
            
            if (!isFileValid)
            {
                _logger.LogWarning("JSON validation failed: {Errors}", string.Join(", ", validationErrors));
            }
            return isFileValid;
        }

        public async Task<string> ReadFileContentAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        public async Task<ClinicalTrial> TransformDataAsync(ClinicalTrial clinicalTrial, CancellationToken token)
        {
            _logger.LogInformation("Starting data transformation for TrialId: {TrialId}", clinicalTrial.TrialId);

            if (clinicalTrial.EndDate == null && clinicalTrial.Status == TrialStatus.Ongoing)
            {
                clinicalTrial.EndDate = clinicalTrial.StartDate.AddMonths(1);
            }

            _logger.LogDebug($"End date calculated for TrialId: {clinicalTrial.TrialId} is {clinicalTrial.EndDate}");

            clinicalTrial.DurationInDays = clinicalTrial.EndDate.HasValue
                ? (clinicalTrial.EndDate.Value - clinicalTrial.StartDate).Days
                : 0;

            _logger.LogDebug($"Duration calculated for TrialId: {clinicalTrial.TrialId} is {clinicalTrial.DurationInDays} days");

            _context.ClinicalTrials.Add(clinicalTrial);
            await _context.SaveChangesAsync(token);

            return clinicalTrial;
        }

        public async Task<ClinicalTrial?> GetClinicTrialByIdAsync(int id, CancellationToken token)
        {
            return await _context.ClinicalTrials.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<IEnumerable<ClinicalTrial>> FilterByStatusAsync(TrialStatus status, CancellationToken token)
        {
            return await _context.ClinicalTrials
                .Where(t => t.Status == status)
                .ToListAsync();
        }
    }
}
