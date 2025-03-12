using System.Text.Encodings.Web;
using InnovateFuture.Application.Auth.Events;
using InnovateFuture.Application.Services.SendEmail;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Auth.Consumers;

public class SendVerificationEventConsumer: IConsumer<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendVerificationEventConsumer> _logger;

    public SendVerificationEventConsumer(IEmailService emailService, IConfiguration configuration, ILogger<SendVerificationEventConsumer> logger)
    {
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        if (context.RoutingKey() == "user.verification")
        {
            _logger.LogInformation($"[SENDING-VERIFICATION-EMAIL] [Start-Sending]{DateTime.UtcNow}");
            try
            {

            
                var message = context.Message;
                // encode token and email
                var encodedToken = UrlEncoder.Default.Encode(message.Token);
                var encodedEmail = UrlEncoder.Default.Encode(message.UserEmail);
                // link
                var verificationLink = message.TokenType switch
                {
                    "email-verification" =>
                        $"{_configuration["FrontEndBaseUrl"]}/auth/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={message.ProfileId}",
                    "reset-password" =>
                        $"{_configuration["FrontEndBaseUrl"]}/auth/reset-password?token={encodedToken}&email={encodedEmail}&pid={message.ProfileId}",
                    _ => throw new ArgumentException("Invalid token type")
                };

                var templatePath = message.RoleEnum != null
                    ? "Templates/InviteEmailTemplate.hbs"
                    : "Templates/RegisterEmailTemplate.hbs";

                var emailData = new
                {
                    userName = message.UserName,
                    verificationLink = verificationLink,
                    roleType = message.RoleEnum?.ToString(),
                };
                string emailBody = _emailService.RenderTemplate(templatePath, emailData);
                await _emailService.SendEmailAsync(message.UserEmail, "Welcome to Innovate future", emailBody);
                _logger.LogInformation($"[SENDING-VERIFICATION-EMAIL] [End-Sending]{DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Verification-Email-Sending-Error]: {ex.Message}");
                throw;
            }
        }
    }
}