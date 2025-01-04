using FluentValidation;

namespace InnovateFuture.Application.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User Id is required.");
        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");
        RuleFor(x => x.Phone)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.");
        RuleFor(x => x.Birthday)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthday must be greater than or equal to now.");
    }
}