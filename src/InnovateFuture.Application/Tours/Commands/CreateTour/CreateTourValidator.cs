using FluentValidation;

namespace InnovateFuture.Application.Tours.Commands.CreateTour;

public class CreateTourValidator : AbstractValidator<CreateTourCommand>
{
    public CreateTourValidator()
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

        RuleFor(x => x.CoverImgUrl)
            .MaximumLength(500).WithMessage("Cover image URL must not exceed 500 characters.")
            .Must(BeAValidUrl).WithMessage("Please enter a valid cover image URL.")
            .When(x => !string.IsNullOrEmpty(x.CoverImgUrl));

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .Must((command, startDate) => startDate < command.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.");
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}