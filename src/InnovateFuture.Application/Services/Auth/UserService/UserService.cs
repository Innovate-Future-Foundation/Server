using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.Auth.UserService;

public class UserService: IUserService
{
    
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
    }

    // Create Organisation Admin and password
    public async Task CreateUserAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errorDetails = result.Errors.Select(e => e.Description);
            throw new Exception($"Failed to create user: {errorDetails}");
        }
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (token == null)
        {
            throw new Exception($"Failed to create email confirmation token");
        }
        return token;
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new Exception($"Failed to update user: {errors}");
        }
    }
}