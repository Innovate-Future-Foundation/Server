using FluentValidation;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;

public class UpdateOrganisationCommandValidator : AbstractValidator<UpdateOrganisationCommand>
{
    public UpdateOrganisationCommandValidator()
    {
        RuleFor(x => x.OrgId)
            .NotEmpty()
            .WithMessage("Organisation ID is required.");

        RuleFor(x => x.OrgName)
            .MinimumLength(2)
            .WithMessage("Organisation name must at least 2 characters.")
            .MaximumLength(200)
            .WithMessage("Organisation name must not exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.OrgName?.Trim()));

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .When(x => !string.IsNullOrEmpty(x.Email?.Trim()));

        RuleFor(x => x.WebsiteUrl)
            .Must(BeAValidUrl)
            .WithMessage("Invalid website URL format.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl?.Trim()));
    }

    // Helper method to validate URL format
    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}