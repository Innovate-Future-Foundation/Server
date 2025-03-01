using FluentValidation;

namespace InnovateFuture.Application.Auth.Commands.Register;

public class RegisterOrganisationAdminValidator: AbstractValidator<RegisterOrganisationAdminCommand>
{
    public RegisterOrganisationAdminValidator()
    {
        RuleFor(x => x.OrgName.Trim())
            .NotEmpty().WithMessage("Organisation name is required.")
            .MinimumLength(2).WithMessage("Organisation name must be at least 2 characters.")
            .MaximumLength(200).WithMessage("Organisation name must not exceed 200 characters.");
        
        RuleFor(x => x.LogoUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid logo URL.")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl?.Trim()));
        
        RuleFor(x => x.WebsiteUrl)
            .Must(BeAValidUrl).WithMessage("Please enter a valid website URL.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl?.Trim()));
        
        RuleFor(x => x.OrgEmail)
            .EmailAddress().WithMessage("Please enter a valid organisation email address.")
            .When(x => !string.IsNullOrEmpty(x.OrgEmail?.Trim()));

        RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("User email is required.")
            .EmailAddress().WithMessage("Please enter a valid user email address.");
        
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .MinimumLength(2).WithMessage("User name must be at least 2 characters.")
            .MaximumLength(100).WithMessage("User name must not exceed 100 characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"(?=.*[!@#$%^&*])").WithMessage("Password must contain at least one special character.");
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}