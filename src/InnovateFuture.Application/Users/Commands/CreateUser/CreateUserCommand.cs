using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserCommand : IRequest<Guid>
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    public Guid ProfileId { get; set; }
    
    public Guid OrgId { get; set; }
    public RoleEnum RoleEnum { get; set; }
    
    public Guid? Inviter { get; set; }
    public Guid? Supervisor { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
}

