using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserCommand : IRequest<Guid>
{
    public Guid OrgId { get; set; }
    
    public RoleEnum RoleEnum { get; set; }
    public Guid? Inviter { get; set; }
    public Guid? Supervisor { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 

}

