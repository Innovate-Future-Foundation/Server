using FluentValidation;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Profile Id is required.");
        
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
                .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        });
        
        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
        });
        
        When(x => !string.IsNullOrEmpty(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.");
        });
        
        When(x => !string.IsNullOrEmpty(x.AvatarUrl), () =>
        {
            RuleFor(x => x.AvatarUrl)
                .MaximumLength(500).WithMessage("Avatar url must not exceed 500 characters.");
        });
    }
}