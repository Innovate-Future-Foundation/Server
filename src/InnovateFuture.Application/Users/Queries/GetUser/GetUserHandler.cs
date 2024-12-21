using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Queries.GetUser;
public class GetUserHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly IUserRepository _UserRepository;
    public GetUserHandler(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    public async Task<User> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _UserRepository.GetByIdAsync(query.UserId);
        return user;
    }
}

