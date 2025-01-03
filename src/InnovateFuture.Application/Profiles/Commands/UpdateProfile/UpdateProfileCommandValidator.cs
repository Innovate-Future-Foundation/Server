using FluentValidation;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("Profile Id is required.");
        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
        RuleFor(x => x.Phone)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.");
        RuleFor(x => x.Avatar)
            .MaximumLength(500).WithMessage("Avatar must not exceed 500 characters.");
    }
}