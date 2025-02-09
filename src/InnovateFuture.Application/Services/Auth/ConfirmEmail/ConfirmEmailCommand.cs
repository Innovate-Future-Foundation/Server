using MediatR;

namespace InnovateFuture.Application.Services.Auth.ConfirmEmail;

public class ConfirmEmailCommand: IRequest<bool>
{
    public string Email { get; set; }
    public string Token { get; set; }
    
    // Get ProfileID from URL
    public Guid ProfileId { get; set; }
}