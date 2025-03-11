using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Application.Services.SendEmail;

public interface IEmailService
{
    Task SendEmailAsync(string receiver, string subject, string body);
    string RenderTemplate(string templatePath, object data);
}