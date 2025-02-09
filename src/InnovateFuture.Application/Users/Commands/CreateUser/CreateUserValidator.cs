using FluentValidation;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.RoleEnum)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role != RoleEnum.UndefinedRole).WithMessage("Role Code provided is not found.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.")
            .When(x=>!string.IsNullOrWhiteSpace(x.FullName));
        RuleFor(x => x.Phone)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.")
            .When(x=>!string.IsNullOrWhiteSpace(x.Phone));
        RuleFor(x => x.Birthday)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthday must be greater than or equal to now.")
            .When(x => x.Birthday != null);
    }
}
