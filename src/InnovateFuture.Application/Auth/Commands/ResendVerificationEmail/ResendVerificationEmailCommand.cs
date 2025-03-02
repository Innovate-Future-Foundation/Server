using MediatR;

namespace InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;

public class ResendVerificationEmailCommand: IRequest<bool>
{
    public string Email { get; set; }
    
    public Guid ProfileId { get; set; }
}