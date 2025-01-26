using ClinicTrialApi.Models;

namespace ClinicTrialApi.Interfaces
{
    public interface IClinicalTrialService
    {
        Task<string> ReadFileContentAsync(IFormFile jsonContent);

        bool ValidateJson(string jsonContent);

        Task<ClinicalTrial> TransformDataAsync(ClinicalTrial clinicalTrial, CancellationToken token);

        Task<ClinicalTrial?> GetClinicTrialByIdAsync(int id, CancellationToken token);

        Task<IEnumerable<ClinicalTrial>> FilterByStatusAsync(TrialStatus status, CancellationToken token);
    }
}
