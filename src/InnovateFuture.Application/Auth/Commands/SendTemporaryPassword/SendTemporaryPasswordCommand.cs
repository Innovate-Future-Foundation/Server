using MediatR;

namespace InnovateFuture.Application.Auth.Commands.SendTemporaryPassword;

public class SendTemporaryPasswordCommand: IRequest<bool>
{
    public string Email { get;}
    public string TemporaryPassword { get;}

    public SendTemporaryPasswordCommand(string email, string temporaryPassword)
    {
        Email = email;
        TemporaryPassword = temporaryPassword;
    }
}