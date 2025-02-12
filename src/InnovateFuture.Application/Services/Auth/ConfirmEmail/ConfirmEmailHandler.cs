using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.Auth.ConfirmEmail;

public class ConfirmEmailHandler: IRequestHandler<ConfirmEmailCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IOrgRepository _orgRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailHandler(UserManager<User> userManager, IOrgRepository orgRepository, IUnitOfWork unitOfWork, IProfileRepository profileRepository)
    {
        _userManager = userManager;
        _orgRepository = orgRepository;
        _unitOfWork = unitOfWork;
        _profileRepository = profileRepository;
    }

    public async Task<bool> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            // 1⃣️ Update User table -> "email_confirmed"
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                throw new Exception($"User with email [{command.Email}] does not exist.");
            }
            var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, command.Token);
            if (!confirmEmailResult.Succeeded)
            {
                throw new Exception($"Invalid or expired token.");
            }
        
            // 2⃣️ Update Profile status
           var profile = await _profileRepository.GetByIdAsync(command.ProfileId, cancellationToken);
           if (profile == null)
           {
               throw new Exception($"Profile [{command.ProfileId}] does not exist.");
           }
           profile.ConfirmRole();
           
            
            // 3⃣️ if role == OrgAdmin then update Organisation status
            if (profile.Role == RoleEnum.OrgAdmin)
            { 
                if (profile.OrgId == null)
                {
                    throw new Exception("Profile does not belong to any organisation.");
                }
                var organisation = await _orgRepository.GetByIdAsync(profile.OrgId.Value, cancellationToken);
                if (organisation?.OrgStatus == OrgStatusEnum.Pending)
                {
                    organisation.ChangeStatus(OrgStatusEnum.Active);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    
}