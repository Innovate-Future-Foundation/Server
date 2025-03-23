using System.Text.Encodings.Web;
using InnovateFuture.Application.Auth.Events;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Application.Services.UserService;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Auth.Consumers;

public class ForgotPasswordEventConsumer: IConsumer<ForgotPasswordEvent>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ForgotPasswordEventConsumer> _logger;
    
    public ForgotPasswordEventConsumer(IUserService userService,  IEmailService emailService, IConfiguration configuration, ILogger<ForgotPasswordEventConsumer> logger)
    {
        _userService = userService;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
    {
        if (context.RoutingKey() == "user.forgot-password")
        {
            _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [Start-Sending] {DateTime.UtcNow}");
            var message = context.Message;
            var user = await _userService.GetUserByEmailAsync(message.UserEmail);
        
            var resetPasswordToken = await _userService.GeneratePasswordResetTokenAsync(user);
        
            var encodedToken = UrlEncoder.Default.Encode(resetPasswordToken);
            var encodedEmail = UrlEncoder.Default.Encode(message.UserEmail);
            var verificationLink =  $"{_configuration["FrontEndBaseUrl"]}/auth/reset-password?token={encodedToken}&email={encodedEmail}";

            var emailData = new
            {
                userName = message.UserEmail,
                verificationLink = verificationLink
            };
            string emailBody = _emailService.RenderTemplate("Templates/ResetPasswordEmailTemplate.hbs", emailData);
            await _emailService.SendEmailAsync(message.UserEmail, "Reset you password", emailBody);
            _logger.LogInformation($"[SENDING-FORGOTPASSWORD-EMAIL] [End-Sending] {DateTime.UtcNow}");
        }
    }
}