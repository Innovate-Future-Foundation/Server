using FluentValidation;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CognitoUuid)
            .NotEmpty().WithMessage("Cognito Uuid is required.");
        RuleFor(x => x.OrgId)
            .NotEmpty().WithMessage("Cognito Org Id is required.");
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Cognito Role Id is required.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.");
        RuleFor(x => x.GivenName)
            .MaximumLength(100).WithMessage("GivenName must not exceed 100 characters.");
        RuleFor(x => x.FamilyName)
            .MaximumLength(100).WithMessage("GivenName must not exceed 100 characters.");
        RuleFor(x => x.Phone)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.");
        RuleFor(x => x.Birthday)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthday must be greater than or equal to now.");
    }
}
