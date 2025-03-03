using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ResetPasswordCommand: IRequest<bool>
{
    public string ResetPasswordToken { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}