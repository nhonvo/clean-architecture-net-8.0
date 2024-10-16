using FluentValidation;

namespace CleanArchitecture.Web.Validations;

public class FileUploadValidator : AbstractValidator<IFormFile>
{
    public FileUploadValidator()
    {
        RuleFor(file => file)
            .NotNull()
            .WithMessage("No file provided.");

        RuleFor(file => file.Length)
            .GreaterThan(0)
            .WithMessage("File is empty.")
            .LessThanOrEqualTo(5 * 1024 * 1024) // 5 MB
            .WithMessage("File size exceeds the limit of 5 MB.");

        RuleFor(file => Path.GetExtension(file.FileName).ToLowerInvariant())
            .Must(ext => new[] { ".jpg", ".jpeg", ".png", ".pdf" }.Contains(ext))
            .WithMessage("File type is not allowed.");
    }
}
