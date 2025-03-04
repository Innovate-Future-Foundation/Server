namespace InnovateFuture.Api.Controllers.Auth;

public class RegisterNormalUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string RoleCode  { get; set; }
}