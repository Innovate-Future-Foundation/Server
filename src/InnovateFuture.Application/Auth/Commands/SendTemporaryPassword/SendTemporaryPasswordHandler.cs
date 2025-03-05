using InnovateFuture.Application.Exceptions;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Auth.Commands.SendTemporaryPassword;

public class SendTemporaryPasswordHandler: IRequestHandler<SendTemporaryPasswordCommand, bool>
{
    private readonly IEmailService _emailService;

    public SendTemporaryPasswordHandler(IEmailService emailService)
    {
        _emailService = emailService;
    } 
    
    public async Task<bool> Handle(SendTemporaryPasswordCommand command, CancellationToken cancellationToken)
    {
        var emailData = new
        {
            userName = command.Email,
            temporaryPassword = command.TemporaryPassword
        };
        
        string emailBody = _emailService.RenderTemplate("Templates/RandomPassword.hbs", emailData);

        await _emailService.SendEmailAsync(command.Email, "Your Temporary Password", emailBody, cancellationToken);
        
        return true;
    }
}