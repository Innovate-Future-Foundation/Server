using FluentValidation;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationCommandValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationCommandValidator()
    {
        RuleFor(x => x.OrgName)
            .NotEmpty().WithMessage("Organisation name is required.")
            .MaximumLength(200).WithMessage("Organisation name must not exceed 200 characters.");
        
        RuleFor(x => x.LogoUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid logo URL.")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));
        
        RuleFor(x => x.WebsiteUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid website URL.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));
        
        RuleFor(x => x.OrgEmail)
            .EmailAddress().WithMessage("Please enter a valid organisation email address.")
            .When(x => !string.IsNullOrEmpty(x.OrgEmail));
        
        RuleFor(x => x.UserEmail)
            .EmailAddress().WithMessage("Please enter a valid user email address.")
            .When(x => !string.IsNullOrEmpty(x.UserEmail));
        RuleFor(x => x.UserName)
            .MaximumLength(100).WithMessage("User name must not exceed 100 characters.");
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}