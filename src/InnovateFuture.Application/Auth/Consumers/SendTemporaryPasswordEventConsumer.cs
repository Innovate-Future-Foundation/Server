using InnovateFuture.Application.Auth.Events;
using InnovateFuture.Application.Services.SendEmail;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Auth.Consumers;

public class SendTemporaryPasswordEventConsumer: IConsumer<TemporaryPasswordEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<SendTemporaryPasswordEventConsumer> _logger;

    public SendTemporaryPasswordEventConsumer(IEmailService emailService, ILogger<SendTemporaryPasswordEventConsumer> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task  Consume(ConsumeContext<TemporaryPasswordEvent> context)
    {
        if (context.RoutingKey() == "user.temporary-password")
        {
            _logger.LogInformation($"[SENDING-TEMPORARY-EMAIL] [Start-Sending] {DateTime.UtcNow}");
            var message = context.Message;
            var emailData = new
            {
                userName = message.UserEmail,
                temporaryPassword = message.TemporaryPassword
            };
        
            string emailBody = _emailService.RenderTemplate("Templates/RandomPassword.hbs", emailData);
            await _emailService.SendEmailAsync(message.UserEmail, "Your Temporary Password", emailBody);
            _logger.LogInformation($"[SENDING-TEMPORARY-EMAIL] [End-Sending] {DateTime.UtcNow}");
        }
    }
}