using System.ComponentModel.DataAnnotations;

namespace ClinicTrialApi.Models
{
    public class ClinicalTrial
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TrialId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Participants must be at least 1.")]
        public int Participants { get; set; }

        [Required]
        public TrialStatus Status { get; set; }
        public int DurationInDays { get; set; }
    }
}
