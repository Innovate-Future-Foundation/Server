using System.Text.Encodings.Web;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Application.Services.UserService;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ForgetPasswordHandler: IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    public ForgetPasswordHandler(IUserService userService,  IEmailService emailService, IConfiguration configuration)
    {
        _userService = userService;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<bool> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ check user exist
        var user = await _userService.GetUserByEmailAsync(command.Email, cancellationToken);
        
        // 2⃣️ generate reset-password-token if user exist
        var resetPasswordToken = await _userService.GeneratePasswordResetTokenAsync(user, cancellationToken);
        
        // 3⃣️ send reset-password email
        var encodedToken = UrlEncoder.Default.Encode(resetPasswordToken);
        var encodedEmail = UrlEncoder.Default.Encode(command.Email);
        var verificationLink =  $"{_configuration["FrontEndBaseUrl"]}/auth/reset-password?token={encodedToken}&email={encodedEmail}";

        var emailData = new
        {
            userName = command.Email,
            verificationLink = verificationLink
        };
        // ender email body
        string emailBody = _emailService.RenderTemplate("Templates/ResetPasswordEmailTemplate.hbs", emailData);
        // send email
        await _emailService.SendEmailAsync(command.Email, "Reset you password", emailBody,
            cancellationToken);
        return true;
    }
}