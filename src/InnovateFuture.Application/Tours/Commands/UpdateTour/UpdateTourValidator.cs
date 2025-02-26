using FluentValidation;

namespace InnovateFuture.Application.Tours.Commands.UpdateTour;

public class UpdateTourValidator : AbstractValidator<UpdateTourCommand>
{
    public UpdateTourValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tour ID is required.");

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

        When(x => !string.IsNullOrEmpty(x.CoverImgUrl), () =>
        {
            RuleFor(x => x.CoverImgUrl)
                .MaximumLength(500).WithMessage("Cover image URL must not exceed 500 characters.")
                .Must(BeAValidUrl).WithMessage("Please enter a valid cover image URL.");
        });

        When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.StartDate!.Value)
                .LessThan(x => x.EndDate!.Value)
                .WithMessage("Start date must be before end date.");
        });
    }

    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}