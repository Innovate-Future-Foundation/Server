namespace InnovateFuture.Api.Controllers.Auth;

public class ConfirmEmailRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
    public Guid ProfileId { get; set; }
}