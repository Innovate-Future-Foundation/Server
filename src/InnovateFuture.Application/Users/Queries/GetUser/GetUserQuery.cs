using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<User>
{
    public Guid UserId { get; set; }
}


