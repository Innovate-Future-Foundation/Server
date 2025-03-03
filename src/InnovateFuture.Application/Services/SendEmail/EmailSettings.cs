namespace InnovateFuture.Application.Services.SendEmail;

public class EmailSettings
{
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderPassword { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpServer { get; set; } = string.Empty;
}
