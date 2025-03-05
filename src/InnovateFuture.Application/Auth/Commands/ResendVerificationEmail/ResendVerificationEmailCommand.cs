using MediatR;

namespace InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;

public class ResendVerificationEmailCommand: IRequest<Unit>
{
    public string Email { get; set; }
}