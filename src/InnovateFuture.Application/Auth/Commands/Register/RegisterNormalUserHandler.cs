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

public class RegisterNormalUserHandler: IRequestHandler<RegisterNormalUserCommand, (Guid ProfileId, User User, string Token, RoleEnum RoleEnum)?>
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
    
    public async Task<(Guid ProfileId, User User, string Token, RoleEnum RoleEnum)?> Handle(RegisterNormalUserCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ check Inviter user is OrgAdmin by using InviterProfileId
        if (await _profileRepository.CheckRoleIsOrgAdminById(command.InviterProfileId, cancellationToken))
        {
            // get orgId by profileId
            var orgId = await _profileRepository.GetOrgIdByProfileIdAsync(command.InviterProfileId, cancellationToken);
            
            // 2⃣️ check organisation exist
            await _orgRepository.CheckIsExistByOrgIdAsync(orgId, cancellationToken);
            
            // 3⃣️ check user exist
            await _userService.CheckUserExistsAsync(command.Email, cancellationToken);
            
            // 4⃣️ generate temperate password
            string temperatePassword = await _userService.GenerateTemperatePassword();
            
            // 5⃣️ ACID transaction for user table, profile table
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
                return (profile.Id, user, token, command.RoleEnum);
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