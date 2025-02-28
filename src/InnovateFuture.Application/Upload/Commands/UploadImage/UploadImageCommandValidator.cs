using FluentValidation;

namespace InnovateFuture.Application.Upload.Commands.UploadImage;
public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{

    private static readonly string[] AllowedContentTypes = { "image/png", "image/jpeg", "image/jpg" };

    public UploadImageCommandValidator()
    {

        RuleFor(x => x.FileStream)
            .NotNull().WithMessage("File is required.")
            .Must(f => f.Length > 0).WithMessage("File cannot be empty.")
            .Must(f => f.Length <= 10 * 1024 * 1024).WithMessage("File size must not exceed 10MB.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .Matches(@"^[a-zA-Z0-9_\-\.]+$").WithMessage("File name contains invalid characters.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("File type is required.")
            .Must(contentType => AllowedContentTypes.Contains(contentType))
            .WithMessage("Only PNG and JPG image files are allowed.");
    }
}