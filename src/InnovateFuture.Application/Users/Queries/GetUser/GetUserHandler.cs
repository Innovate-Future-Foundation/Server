using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Queries.GetUser;
public class GetUserHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly IUserRepository _userRepository;
    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.UserId);
        return user;
    }
}

