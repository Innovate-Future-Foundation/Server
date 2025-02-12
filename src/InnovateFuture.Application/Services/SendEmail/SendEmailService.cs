using System.Text.Encodings.Web;
using HandlebarsDotNet;
using InnovateFuture.Application.Services.SendEmail;
using MimeKit;
using MailKit.Net.Smtp;
using InnovateFuture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace InnovateFuture.Application.Services.SendEmail;

public class SendEmailService: IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly UserManager<User> _userManager;

    public SendEmailService(IOptions<EmailSettings> emailSettings, UserManager<User> userManager)
    {
        _emailSettings = emailSettings.Value;
        _userManager = userManager;
    }

    public async Task SendEmailAsync(string receiver, string subject, string body, CancellationToken cancellationToken)
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
                await smtpClient.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls,
                    cancellationToken);
                // Authentication
                await smtpClient.AuthenticateAsync(senderEmail, senderPassword, cancellationToken);
                // Send email
                await smtpClient.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send email failed: {ex.Message}");
                throw;
            }
            finally
            {
                // Disconnect
                await smtpClient.DisconnectAsync(true, cancellationToken);
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

    public async Task SendVerificationEmailAsync(User user, Guid profileId)
    {
        // generate verification token
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        // make the token suitable for url format
        var encodedToken = UrlEncoder.Default.Encode(token);
        var encodedEmail = UrlEncoder.Default.Encode(user.Email!);
        // Fr
        var verificationLink = $"http://localhost:5173/signup/email-verification?token={encodedToken}&email={encodedEmail}&pid={profileId}";
        // generate email body
        var emailData = new
        {
            name = user.UserName,
            verificationLink = verificationLink
        };
        // send email
        string templatePath = "Templates/RegisterEmailTemplate.hbs";
        string emailBody = RenderTemplate(templatePath, emailData);
        await SendEmailAsync(user.Email!, "Welcome to Innovate feature", emailBody, CancellationToken.None);
    }
}