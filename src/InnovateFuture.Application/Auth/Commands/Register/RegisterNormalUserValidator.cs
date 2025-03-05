using FluentValidation;

namespace InnovateFuture.Application.Services.Auth.Register;

public class RegisterNormalUserValidator: AbstractValidator<RegisterNormalUserCommand>
{
    public RegisterNormalUserValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Please enter a valid user email address.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}