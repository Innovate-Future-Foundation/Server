using FluentValidation;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(2).WithMessage("Username must be at least 2 characters.")
            .MaximumLength(100).WithMessage("User name must not exceed 100 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        
        RuleFor(x => x.RoleEnum)
            .NotEmpty().WithMessage("Role is required.")
            .Must(role => role != RoleEnum.UndefinedRole).WithMessage("Role Code provided is not found.");

        RuleFor(x => x.OrgId)
            .NotEmpty().WithMessage("Organisation Id is required.");
        
        // Password already handle in Identity package
    }
}
