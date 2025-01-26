using System.ComponentModel.DataAnnotations;

namespace ClinicTrialApi.Models
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
