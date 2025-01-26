using ClinicTrialApi.Models;
using FluentValidation;

namespace ClinicTrialApi.Validators
{
    public class FileUploadValidator : AbstractValidator<FileUploadRequest>
    {
        public FileUploadValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.")
                .Must(f => f.ContentType == "application/json")
                .WithMessage("Only JSON files are allowed.")
                .Must(f => f.Length <= 1 * 1024 * 1024) // 1 MB limit
                .WithMessage("File size must be less than 1 MB.");
        }
    }
}
