using System.Text.Encodings.Web;
using InnovateFuture.Application.Exceptions;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;

public class ResendVerificationEmailHandler: IRequestHandler<ResendVerificationEmailCommand,Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public ResendVerificationEmailHandler(UserManager<User> userManager, IEmailService emailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
    }
    public async Task<Unit> Handle(ResendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ get user by email
        var user = await _userManager.FindByEmailAsync(command.Email);
        
        if (user == null)
        {
            throw new IFApplicationNotFoundException($"User with email {command.Email} does not exist.");
        }
        
        // 2⃣️ check if user already verified
        if (user is { EmailConfirmed: true })
        {
            throw new IFApplicationBusinessException($"User with email {command.Email} has already been verified.");
        }
        
        // 3⃣️ generate verification token by using Identity service
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        // 4⃣️ send email
        var encodedToken = UrlEncoder.Default.Encode(token);
        var encodedEmail = UrlEncoder.Default.Encode(user.Email!);
        // link
        var verificationLink =
            $"{_configuration["FrontEndBaseUrl"]}/auth/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={user.DefaultProfileId}";
        // generate email body
        var emailData = new
        {
            userName = user.UserName,
            verificationLink,
        };
        
        string emailBody = _emailService.RenderTemplate("Templates/RegisterEmailTemplate.hbs", emailData);
        // send email
        await _emailService.SendEmailAsync(user.Email!, "Welcome to Innovate future", emailBody,
            cancellationToken);
        
        return Unit.Value;
    }
}