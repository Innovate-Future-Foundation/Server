using System.Security.Authentication;
using InnovateFuture.Application.Services.UserService;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.Auth.Register;

public class RegisterNormalUserHandler: IRequestHandler<RegisterNormalUserCommand, (string UserName, string UserEmail, Guid ProfileId, string Token, RoleEnum RoleEnum)?>
{
    private readonly IOrgRepository _orgRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;


    public RegisterNormalUserHandler( UserManager<User> userManager, IOrgRepository orgRepository, IProfileRepository profileRepository, IUnitOfWork unitOfWork,
        IUserService userService)
    {
        _orgRepository = orgRepository;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _userManager = userManager;
    }
    
    public async Task<(string UserName, string UserEmail, Guid ProfileId, string Token, RoleEnum RoleEnum)?> Handle(RegisterNormalUserCommand command, CancellationToken cancellationToken)
    {
        if (await _profileRepository.CanInviteRole(command.InviterProfileId, command.RoleEnum, cancellationToken))
        {
            var orgId = await _profileRepository.GetOrgIdByProfileIdAsync(command.InviterProfileId, cancellationToken);
            
            await _orgRepository.CheckIsExistByOrgIdAsync(orgId, cancellationToken);
            
            await _userService.CheckUserExistsAsync(command.Email, cancellationToken);
            
            string temperatePassword = await _userService.GenerateTemperatePassword();
            
            using var transactions = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User(
                    userName: command.Name,
                    email: command.Email
                );
                await _userService.CreateUserAsync(user, temperatePassword, cancellationToken);
    
                var profile = new Profile(
                    userId: user.Id,
                    role: command.RoleEnum,
                    orgId: orgId,
                    name: command.Name,
                    email: command.Email,
                    inviter: command.InviterProfileId
                );
                profile.AddUser(user);
                user.UpdateDefaultProfile(profile.Id);
                await _profileRepository.AddAsync(profile, cancellationToken);
                
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
    
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return (user.UserName, user.Email, profile.Id, token, command.RoleEnum);
            }
            catch (Exception ex)
            {
                try
                {
                    await _unitOfWork.RollbackTransactionAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new IFDatabaseException($"Transaction rollback failed: {rollbackEx.Message}");
                }
                throw;
            }
        }
        throw new UnauthorizedAccessException("Invite user must be organisation administrator.");
    }
}