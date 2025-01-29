using FluentValidation;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;

public class UpdateOrganisationCommandValidator : AbstractValidator<UpdateOrganisationCommand>
{
    public UpdateOrganisationCommandValidator()
    {
        // Organisation ID is the only required field as we need to know which organisation to update
        RuleFor(x => x.OrgId)
            .NotEmpty()
            .WithMessage("Organisation ID is required.");

        // If organisation name is provided, it must not exceed 100 characters
        When(x => !string.IsNullOrEmpty(x.OrgName), () =>
        {
            RuleFor(x => x.OrgName)
                .MaximumLength(100)
                .WithMessage("Organisation name must not exceed 100 characters.");
        });

        // If email is provided, it must be in a valid email format
        // Example: example@domain.com
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.");
        });

        // If website URL is provided, it must be in a valid URL format
        // Example: https://www.example.com
        When(x => !string.IsNullOrEmpty(x.WebsiteUrl), () =>
        {
            RuleFor(x => x.WebsiteUrl)
                .Must(BeAValidUrl)
                .WithMessage("Invalid website URL format.");
        });
    }

    // Helper method to validate URL format
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}