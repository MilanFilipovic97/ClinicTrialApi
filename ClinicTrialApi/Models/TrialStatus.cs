using System.ComponentModel.DataAnnotations;

namespace ClinicTrialApi.Models
{
    public enum TrialStatus
    {
        [Display(Name = "Not Started")]
        NotStarted = 0,

        [Display(Name = "Ongoing")]
        Ongoing = 1,

        [Display(Name = "Completed")]
        Completed = 2
    }
}
