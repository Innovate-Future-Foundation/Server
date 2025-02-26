using FluentValidation;

namespace InnovateFuture.Application.Activities.Commands.UpdateActivity;

public class UpdateActivityValidator : AbstractValidator<UpdateActivityCommand>
{
    public UpdateActivityValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Activity ID is required.");

        When(x => !string.IsNullOrEmpty(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
        });

        When(x => !string.IsNullOrEmpty(x.Comment), () =>
        {
            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.");
        });

        When(x => !string.IsNullOrEmpty(x.Summary), () =>
        {
            RuleFor(x => x.Summary)
                .MaximumLength(500).WithMessage("Summary must not exceed 500 characters.");
        });

        When(x => !string.IsNullOrEmpty(x.Location), () =>
        {
            RuleFor(x => x.Location)
                .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");
        });

        When(x => !string.IsNullOrEmpty(x.CoverImgUrl), () =>
        {
            RuleFor(x => x.CoverImgUrl)
                .MaximumLength(500).WithMessage("Cover image URL must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("Please enter a valid cover image URL.");
        });

        When(x => x.StartTime.HasValue && x.EndTime.HasValue, () =>
        {
            RuleFor(x => x.StartTime!.Value)
                .LessThan(x => x.EndTime!.Value)
                .WithMessage("Start time must be before end time.");
        });
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}