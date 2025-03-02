using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ResetPasswordCommand: IRequest<bool>
{
    public string NewPassword { get; set; }
    public Guid ProfileId { get; set; }
}