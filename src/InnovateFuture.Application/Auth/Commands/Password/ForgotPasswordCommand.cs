using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ForgotPasswordCommand: IRequest<bool>
{
    public string Email { get; set; }
}