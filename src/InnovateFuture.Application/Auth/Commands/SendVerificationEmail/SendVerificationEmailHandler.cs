using System.Text.Encodings.Web;
using System.Text.Json;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InnovateFuture.Application.Auth.Commands.SendVerificationEmail;

public class SendVerificationEmailHandler: IRequestHandler<SendVerificationEmailCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public SendVerificationEmailHandler(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<bool> Handle(SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // encode token and email
            var encodedToken = UrlEncoder.Default.Encode(command.Token);
            var encodedEmail = UrlEncoder.Default.Encode(command.User.Email);
            // link
            var verificationLink = command.TokenType switch
            {
                "email-verification" =>
                    $"{_configuration["FrontEndBaseUrl"]}/auth/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={command.ProfileId}",
                "reset-password" =>
                    $"{_configuration["FrontEndBaseUrl"]}/auth/reset-password?token={encodedToken}&email={encodedEmail}&pid={command.ProfileId}",
                _ => throw new ArgumentException("Invalid token type")
            };

            var templatePath = command.RoleEnum != null
                ? "Templates/InviteEmailTemplate.hbs"
                : "Templates/RegisterEmailTemplate.hbs";

            var emailData = new
            {
                userName = command.User.UserName,
                verificationLink = verificationLink,
                roleType = command.RoleEnum?.ToString(),
            };
            string emailBody = _emailService.RenderTemplate(templatePath, emailData);
            await _emailService.SendEmailAsync(command.User.Email, "Welcome to Innovate future", emailBody, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Verification-Email-Sending-Error]: {ex.Message}");
            throw;
        }
    }
}