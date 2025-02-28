using FluentValidation;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Profile Id is required.");
        
        RuleFor(x => x.Email)
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .EmailAddress().WithMessage("Kindly enter a valid Email Address.")
                .When(x => !string.IsNullOrEmpty(x.Email?.Trim()));

        RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name?.Trim()));
        
        RuleFor(x => x.Phone)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.")
            .When(x => !string.IsNullOrEmpty(x.Phone?.Trim()));
        
        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500).WithMessage("Avatar url must not exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.AvatarUrl?.Trim()));
    }
}