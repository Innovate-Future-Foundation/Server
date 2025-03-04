using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Services.UserService;

public interface IUserService
{
    Task CreateUserAsync(User user, string password, CancellationToken cancellationToken = default);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(User user, string newPassword, string resetPasswordToken, CancellationToken cancellationToken = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<string> GeneratePasswordResetTokenAsync(User user, CancellationToken cancellationToken = default);
    Task CheckUserExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<string> GenerateTemperatePassword();
}