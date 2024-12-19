using FluentValidation;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
public class CreateOrganisationValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationValidator()
    {
        RuleFor(x => x.OrgName)
            .NotEmpty().WithMessage("Organisation name is required.")
            .MaximumLength(255).WithMessage("Organisation name cannot exceed 255 characters.");

        RuleFor(x => x.LogoUrl)
            .MaximumLength(255).WithMessage("Logo URL cannot exceed 255 characters.")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));

        RuleFor(x => x.WebsiteUrl)
            .MaximumLength(255).WithMessage("Website URL cannot exceed 255 characters.")
            .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(x => x.Subscription)
            .MaximumLength(50).WithMessage("Subscription type cannot exceed 50 characters.");

        RuleFor(x => x.Status)
            .IsEnumName(typeof(OrganisationStatus), caseSensitive: false)
            .When(x => !string.IsNullOrEmpty(x.Status))
            .WithMessage("Invalid organisation status.");
    }
}