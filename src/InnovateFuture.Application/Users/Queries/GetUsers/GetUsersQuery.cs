using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<IEnumerable<User>>
{
    public Guid? CognitoUuid { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime? Birthday { get; set; } 
}


