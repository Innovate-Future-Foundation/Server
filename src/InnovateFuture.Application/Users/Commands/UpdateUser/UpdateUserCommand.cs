using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InnovateFuture.Application.Users.Commands.UpdateUser;
public class UpdateUserCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid? CognitoUuid { get; set; }
    public Guid? DefaultProfile{ get; set;}
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}

