using FluentValidation;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationCommandValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationCommandValidator()
    {
        RuleFor(x => x.OrgName.Trim())
            .NotEmpty().WithMessage("Organisation name is required.")
            .MinimumLength(2).WithMessage("Organisation name must be between 2 and 200 characters.")
            .MaximumLength(200).WithMessage("Organisation name must not exceed 200 characters.");
        
        RuleFor(x => x.LogoUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid logo URL.")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl?.Trim()));
        
        RuleFor(x => x.WebsiteUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid website URL.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl?.Trim()));
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Please enter a valid organisation email address.")
            .When(x => !string.IsNullOrEmpty(x.Email?.Trim()));
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}