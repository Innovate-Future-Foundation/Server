using FluentValidation;

namespace InnovateFuture.Application.Auth.Commands.Login;

public class LoginValidator:  AbstractValidator<LoginCommand>
{
   public LoginValidator()
   {
   }
}