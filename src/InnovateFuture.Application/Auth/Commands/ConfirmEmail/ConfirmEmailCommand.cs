using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Auth.Commands.ConfirmEmail;

public class ConfirmEmailCommand: IRequest<(string Email, string Result, bool IsOrgAdmin)>
{
    public string Email { get; set; }
    public string Token { get; set; }
    
    // Get ProfileID from URL
    public Guid ProfileId { get; set; }
}