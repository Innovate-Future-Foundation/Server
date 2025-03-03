namespace InnovateFuture.Api.Controllers.Auth;

public class ResetPasswordRequest
{
    public string ResetPasswordToken { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}