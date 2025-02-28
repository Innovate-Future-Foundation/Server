using FluentValidation;

namespace InnovateFuture.Application.Auth.Login;

public class LoginValidator:  AbstractValidator<LoginCommand>
{
   public LoginValidator()
   {
   }
}