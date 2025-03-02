namespace InnovateFuture.Api.Controllers.Auth;

public class ResendVerficationEmailRequest
{
    public string Email { get; set; }
    public Guid ProfileId { get; set; }
}