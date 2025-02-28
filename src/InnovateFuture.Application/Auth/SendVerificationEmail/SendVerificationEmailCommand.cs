using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Auth.SendVerificationEmail;

public class SendVerificationEmailCommand: IRequest<bool>
{
    public User User { get; }
    public Guid ProfileId { get; }

    public SendVerificationEmailCommand(User user, Guid profileId)
    {
        User = user;
        ProfileId = profileId;
    }
}