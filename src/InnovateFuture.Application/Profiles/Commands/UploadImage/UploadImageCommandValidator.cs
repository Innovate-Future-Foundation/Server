using FluentValidation;

namespace InnovateFuture.Application.Profiles.Commands.UploadImage;
public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
{
    private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/jpg" };

    public UploadImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Profile Id is required.");
        
        When(x => !string.IsNullOrEmpty(x.FileName), () =>
        {
            RuleFor(x => x.FileName)
                .MaximumLength(500).WithMessage("FileName must not exceed 500 characters.");
        });
        
        When(x => !string.IsNullOrEmpty(x.ContentType), () =>
        {
            RuleFor(x => x.ContentType)
                .Must(ct => AllowedContentTypes.Contains(ct.ToLower()))
                .WithMessage("ContentType must be 'image/jpeg' or 'image/jpg'.");
        });
        
        When(x => x.FileStream != null && x.FileStream.CanRead, () =>
        {
            RuleFor(x => x.FileStream.Length)
                .LessThanOrEqualTo(10 * 1024 * 1024) // 10MB file size limit
                .WithMessage("File size must not exceed 5MB.");
        });
    }
}