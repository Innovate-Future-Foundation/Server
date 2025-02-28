using FluentValidation;

namespace InnovateFuture.Application.Services.Auth.Login;

public class LoginValidator:  AbstractValidator<LoginCommand>
{
   public LoginValidator()
   {
      RuleFor(x => x.Email)
         .NotEmpty().EmailAddress().WithMessage("Email canot be empty")
         .EmailAddress().WithMessage("Please enter a valid user email address.");
      
      RuleFor(x => x.Password)
         .NotEmpty().WithMessage("Password is required.")
         .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
         .Matches(@"(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
         .Matches(@"(?=.*[!@#$%^&*])").WithMessage("Password must contain at least one special character.");
   }
}