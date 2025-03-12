using System.Text.Encodings.Web;
using InnovateFuture.Application.Auth.Events;
using InnovateFuture.Application.Exceptions;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Auth.Consumers;

public class ResendUserRegisteredEventConsumer: IConsumer<ResendUserRegisteredEvent>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ResendUserRegisteredEventConsumer> _logger;

    public ResendUserRegisteredEventConsumer(UserManager<User> userManager, IEmailService emailService, IConfiguration configuration, ILogger<ResendUserRegisteredEventConsumer> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<ResendUserRegisteredEvent> context)
    {
        if (context.RoutingKey() == "user.resend-verification")
        {
            _logger.LogInformation($"[RESENDING-EMAIL] [Start-Sending] {DateTime.UtcNow}");
            var message = context.Message;
            var user = await _userManager.FindByEmailAsync(message.UserEmail);
            if (user == null)
            {
                throw new IFApplicationNotFoundException($"User with email {message.UserEmail} does not exist.");
            }
            if (user is { EmailConfirmed: true })
            {
                throw new IFApplicationBusinessException($"User with email {message.UserEmail} has already been verified.");
            }
        
        
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = UrlEncoder.Default.Encode(token);
            var encodedEmail = UrlEncoder.Default.Encode(user.Email!);
            var verificationLink =
                $"{_configuration["FrontEndBaseUrl"]}/auth/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={user.DefaultProfileId}";
            var emailData = new
            {
                userName = user.UserName,
                verificationLink,
            };
            string emailBody = _emailService.RenderTemplate("Templates/RegisterEmailTemplate.hbs", emailData);
            await _emailService.SendEmailAsync(user.Email!, "Welcome to Innovate future", emailBody);
            _logger.LogInformation($"[RESENDING-EMAIL] [End-Sending] {DateTime.UtcNow}");
        }
    }
}