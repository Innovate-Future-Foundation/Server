using FluentValidation;

namespace InnovateFuture.Application.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.")
            .EmailAddress().WithMessage("Kindly enter a valid Email Address.")
            .When(x => string.IsNullOrEmpty(x.Email?.Trim()));
        
        RuleFor(x => x.FullName)
            .MinimumLength(2).WithMessage("Full name must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.")
            .When(x => string.IsNullOrEmpty(x.FullName?.Trim()));
        
        RuleFor(x => x.PhoneNumber)
            .Matches("^\\+61\\s4\\d{8}$").WithMessage("Kindly enter a valid AU Phone Number.")
            .When(x => string.IsNullOrEmpty(x.PhoneNumber?.Trim()));

        RuleFor(x => x.Birthday)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Birthday must be greater than or equal to now.")
            .When(x => x.Birthday is not null);
    }
}