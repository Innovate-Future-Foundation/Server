using System.Text.Encodings.Web;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.Auth.SendVerificationEmail;

public class SendVerificationEmailHandler: IRequestHandler<SendVerificationEmailCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;

    public SendVerificationEmailHandler(IEmailService emailService, UserManager<User> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<bool> Handle(SendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // generate verification token by using Identity service
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(command.User);
            // encode token and email
            var encodedToken = UrlEncoder.Default.Encode(token);
            var encodedEmail = UrlEncoder.Default.Encode(command.User.Email);
            // link
            var verificationLink =
                $"http://localhost:5173/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={command.ProfileId}";
            // generate email body
            var emailData = new
            {
                userName = command.User.UserName,
                verificationLink = verificationLink,
            };
            string emailBody = _emailService.RenderTemplate("Templates/RegisterEmailTemplate.hbs", emailData);
            // send email
            await _emailService.SendEmailAsync(command.User.Email, "Welcome to Innovate future", emailBody,
                cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Verification-Email-Sending-Error]: {ex.Message}");
            return false;
        }
    }
}