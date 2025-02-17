using FluentValidation;

namespace InnovateFuture.Application.Services.Auth.Login;

public class LoginValidator:  AbstractValidator<LoginCommand>
{
   public LoginValidator()
   {
      RuleFor(x => x.Email)
         .EmailAddress().WithMessage("Please enter a valid user email address.")
         .When(x => !string.IsNullOrEmpty(x.Email));
   }
}