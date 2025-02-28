using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Services.UserService;

public interface IUserService
{
    Task CreateUserAsync(User user, string password, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
}