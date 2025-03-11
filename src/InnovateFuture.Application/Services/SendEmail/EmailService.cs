using HandlebarsDotNet;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;

namespace InnovateFuture.Application.Services.SendEmail;

public class EmailService: IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string receiver, string subject, string body)
    {
        var senderEmail = _emailSettings.SenderEmail;
        var senderName = _emailSettings.SenderName;
        var senderPassword = _emailSettings.SenderPassword;
        var smtpServer = _emailSettings.SmtpServer;
        var smtpPort = _emailSettings.SmtpPort;
        
        // Create email message
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
        emailMessage.To.Add(new MailboxAddress(receiver, receiver));
        emailMessage.Subject = subject;
        
        // create email body
        var bodyBuilder =  new BodyBuilder { HtmlBody = body };
        emailMessage.Body = bodyBuilder.ToMessageBody();
        
        // send email
        using (var smtpClient = new SmtpClient())
        {
            try
            {
                // Connect to SMTP Server
                await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                // Authentication
                await smtpClient.AuthenticateAsync(senderEmail, senderPassword);
                // Send email
                await smtpClient.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send email failed: {ex.Message}");
                throw;
            }
            finally
            {
                // Disconnect
                await smtpClient.DisconnectAsync(true);
            }
        }

    }

    public string RenderTemplate(string templatePath, object data)
    {
        // Load html 
        var template = File.ReadAllText(templatePath);
        // compile html by using handlebars
        var compiledTemplate = Handlebars.Compile(template);
        // render html with data
        return compiledTemplate(data);
    }
}