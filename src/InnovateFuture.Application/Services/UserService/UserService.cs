using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Exceptions;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.UserService;

public class UserService: IUserService
{
    
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager)
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
            throw new IFBusinessRuleViolationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
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


    public async Task ResetPasswordAsync(User user, string newPassword, CancellationToken cancellationToken = default)
    {
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            if (errors.Any(e => e.Contains("token", StringComparison.OrdinalIgnoreCase)))
            {
                throw new IFUnauthorizedActionException($"Invalid or expired reset token: {string.Join(", ", errors)}");
            }

            if (errors.Any(e => e.Contains("password", StringComparison.OrdinalIgnoreCase)))
            {
                throw new IFDomainValidationException($"Weak or invalid password: {string.Join(", ", errors)}");
            }
            throw new Exception($"Failed to reset password: {string.Join(", ", errors)}");
        }
    }
}