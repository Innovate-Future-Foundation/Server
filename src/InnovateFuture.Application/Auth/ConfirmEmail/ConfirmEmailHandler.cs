using InnovateFuture.Application.Services.Security.TokenService;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Domain.Exceptions;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Auth.ConfirmEmail;

public class ConfirmEmailHandler: IRequestHandler<ConfirmEmailCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IOrgRepository _orgRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public ConfirmEmailHandler(UserManager<User> userManager, IOrgRepository orgRepository, IUnitOfWork unitOfWork, IProfileRepository profileRepository, ITokenService tokenService)
    {
        _userManager = userManager;
        _orgRepository = orgRepository;
        _unitOfWork = unitOfWork;
        _profileRepository = profileRepository;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            // 1⃣️ Update User table -> "email_confirmed"
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                throw new IFEntityNotFoundException("User", nameof(command.Email));
            }
            var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, command.Token);
            if (!confirmEmailResult.Succeeded)
            {
                throw new IFBusinessRuleViolationException($"Invalid or expired token.");
            } 
            
            // 2⃣️ Update Profile status
           var profile = await _profileRepository.GetByIdAsync(command.ProfileId, cancellationToken);
           profile.ConfirmRole();
           
            // 3⃣️ if role == OrgAdmin then update Organisation status
            if (profile.Role == RoleEnum.OrgAdmin)
            { 
                if (profile.OrgId == null)
                {
                    throw new IFEntityNotFoundException("OrgId in Profile", nameof(command.ProfileId));
                }
                var organisation = await _orgRepository.GetByIdAsync(profile.OrgId.Value, cancellationToken);
                if (organisation?.OrgStatus == OrgStatusEnum.Pending)
                {
                    organisation.ChangeStatus(OrgStatusEnum.Active);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            
            // 4⃣️ Generate Token
            var accessToken = await _tokenService.GenerateJwtTokenAsync(command.ProfileId);
            Console.WriteLine("accessToken",accessToken);
            return accessToken;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}