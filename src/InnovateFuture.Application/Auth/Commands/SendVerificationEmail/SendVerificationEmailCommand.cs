using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Auth.Commands.SendVerificationEmail;

public class SendVerificationEmailCommand: IRequest<bool>
{
    public User User { get; }
    public Guid ProfileId { get; }
    public string Token { get; }
    public string TokenType { get;  }
    public RoleEnum? RoleEnum { get; }

    public SendVerificationEmailCommand(User user, Guid profileId, string token, string tokenType, RoleEnum? roleEnum = null)
    {
        User = user;
        ProfileId = profileId;
        Token = token;
        TokenType = tokenType;
        RoleEnum = roleEnum;
    }
}