using System.Text.Encodings.Web;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;

public class ResendVerificationEmailHandler: IRequestHandler<ResendVerificationEmailCommand, bool>
{
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public ResendVerificationEmailHandler(IProfileRepository profileRepository, UserManager<User> userManager, IEmailService emailService)
    {
        _profileRepository = profileRepository;
        _userManager = userManager;
        _emailService = emailService;
    }


    public async Task<bool> Handle(ResendVerificationEmailCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ get user by profileId
        var user = await _profileRepository.GetUserByProfileId(command.ProfileId, cancellationToken);
        
        // 2⃣️ check if user already verified
        if (user.EmailConfirmed)
        {
            return false;
        }
        
        // 3⃣️ generate verification token by using Identity service
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        // 4⃣️ send email
        var encodedToken = UrlEncoder.Default.Encode(token);
        var encodedEmail = UrlEncoder.Default.Encode(user.Email!);
        // link
        var verificationLink =
            $"http://localhost:5173/auth/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={command.ProfileId}";
        // generate email body
        var emailData = new
        {
            userName = user.UserName,
            verificationLink = verificationLink,
        };
        string emailBody = _emailService.RenderTemplate("Templates/RegisterEmailTemplate.hbs", emailData);
        // send email
        await _emailService.SendEmailAsync(user.Email!, "Welcome to Innovate future", emailBody,
            cancellationToken);
        return true;
    }
}