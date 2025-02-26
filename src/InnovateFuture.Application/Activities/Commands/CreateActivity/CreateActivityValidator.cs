using FluentValidation;

namespace InnovateFuture.Application.Activities.Commands.CreateActivity;

public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityValidator()
    {
        RuleFor(x => x.OrgId)
            .NotEmpty().WithMessage("Organisation ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));

        RuleFor(x => x.Summary)
            .MaximumLength(500).WithMessage("Summary must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Summary));

        RuleFor(x => x.Location)
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.CoverImgUrl)
            .MaximumLength(500).WithMessage("Cover image URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("Please enter a valid cover image URL.")
            .When(x => !string.IsNullOrEmpty(x.CoverImgUrl));

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.")
            .Must((command, startTime) => startTime < command.EndTime)
            .WithMessage("Start time must be before end time.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.");
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}