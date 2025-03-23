using InnovateFuture.Application.Services.Security.TokenService;
using InnovateFuture.Application.Services.UserService;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Domain.Exceptions;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Auth.Commands.ConfirmEmail;

public class ConfirmEmailHandler: IRequestHandler<ConfirmEmailCommand, (string Email, string Result, bool IsOrgAdmin)>
{
    private readonly UserManager<User> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IOrgRepository _orgRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly ILogger<ConfirmEmailHandler> _logger;

    public ConfirmEmailHandler(UserManager<User> userManager, IOrgRepository orgRepository, IUnitOfWork unitOfWork, IProfileRepository profileRepository, ITokenService tokenService, IUserService userService, ILogger<ConfirmEmailHandler> logger)
    {
        _userManager = userManager;
        _orgRepository = orgRepository;
        _unitOfWork = unitOfWork;
        _profileRepository = profileRepository;
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<(string Email,string Result, bool IsOrgAdmin)> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _logger.LogInformation("Confirm email operation started");
        try
        {
            // 1⃣️ Update User table -> "email_confirmed"
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                throw new IFEntityNotFoundException("User", nameof(command.Email));
            }
            _logger.LogInformation($"HERE-USER {user}");
            var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, command.Token);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (!confirmEmailResult.Succeeded)
            {
                throw new IFBusinessRuleViolationException($"Invalid or expired token.");
            } 
            _logger.LogInformation($"HERE-USER-EMAIL {user.EmailConfirmed}");
            
            // 2⃣️ Update Profile status
           var profile = await _profileRepository.GetByIdAsync(command.ProfileId, cancellationToken);
           profile.ConfirmRole();
           
            // 3⃣️ if role == OrgAdmin then update Organisation status
            try
            {
                if (profile.Role == RoleEnum.OrgAdmin)
                {
                    var organisation = await _orgRepository.GetByIdAsync(profile.OrgId!.Value, cancellationToken);
                    if (organisation?.OrgStatus == OrgStatusEnum.Pending)
                    {
                        organisation.ChangeStatus(OrgStatusEnum.Active);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync();
                    // 4⃣️ Generate Token for directly to dashboard
                    var accessToken = await _tokenService.GenerateJwtTokenAsync(command.ProfileId);
                    _logger.LogInformation($"HERE-FINAL {user.Email} {accessToken}");
                    return (user.Email, accessToken, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HERE-FINAL {user.Email} {ex.Message}");
            }
            
            // 5⃣️ if user.Role != OrgAdmin
            string temporaryPassword = await _userService.GenerateTemperatePassword();
            string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userService.ResetPasswordAsync(user, temporaryPassword, resetPasswordToken, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            
            return (user.Email, temporaryPassword, false);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}