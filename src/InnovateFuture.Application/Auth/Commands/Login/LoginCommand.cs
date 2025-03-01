using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Login;

public class LoginCommand: IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}