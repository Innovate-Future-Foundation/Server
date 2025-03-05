using MediatR;

namespace InnovateFuture.Application.Auth.SendTemporaryPasswordEmail;

public class SendTemporaryPasswordEmailCommand: IRequest<bool>
{
    public Guid UserId { get; }

    public SendTemporaryPasswordEmailCommand(Guid userId)
    {
        UserId = userId;
    }
}