using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Application.Services.Auth.UserService;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using Microsoft.Extensions.Logging;

namespace InnovateFuture.Application.Organisations.Commands.CreateOrganisation;

public class CreateOrganisationHandler : IRequestHandler<CreateOrganisationCommand, Guid>
{
    private readonly IOrgRepository _orgRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    
    private readonly ILogger<CreateOrganisationHandler> _logger;

    public CreateOrganisationHandler(IOrgRepository orgRepository, IProfileRepository profileRepository,  IEmailService emailService, IUnitOfWork unitOfWork, IUserService userService, ILogger<CreateOrganisationHandler> logger)
    {
        _orgRepository = orgRepository;
        _profileRepository = profileRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _logger = logger;
    }
    
    // Use ACID transactionLogoUrl
    // Create Organisation, Organisation Admin and Profile at same time
    // TODO: Validate company not repeat
    public async Task<Guid> Handle(CreateOrganisationCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1⃣️ Create Organisation
            var organisation = new Organisation(
                command.OrgName,
                null,
                command.LogoUrl,
                command.WebsiteUrl,
                command.Address,
                command.OrgEmail,
                command.Subscription
            );
            await _orgRepository.AddAsync(organisation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // 2⃣️ Create Organisation Admin
            var organisationAdmin = new User(
                userName: command.UserName,
                email: command.UserEmail
            );
            await _userService.CreateUserAsync(organisationAdmin, command.Password, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // 3⃣️ Create Organisation Admin Profile
            var profile = new Profile(
                userId: organisationAdmin.Id,
                role: RoleEnum.OrgAdmin,
                orgId: organisation.OrgId,
                name: command.UserName,
                email: command.UserEmail
            );
            await _profileRepository.AddAsync(profile, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            var savedProfile = await _profileRepository.GetByIdAsync(profile.Id, cancellationToken);
            if (savedProfile == null)
            {
                throw new Exception($"[Debug-savedProfileId]:{savedProfile.Id} not found");
            }
            
            _logger.LogDebug("[Debug] Saved Profile ID: {ProfileId}", savedProfile.Id);
            
            // 4⃣️ Update User table DefaultProfileId
            organisationAdmin.UpdateProfile(profile.Id);
            await _userService.UpdateUserAsync(organisationAdmin, cancellationToken);

            // 5⃣️ Send Email
            await _emailService.SendVerificationEmailAsync(organisationAdmin, savedProfile.Id);
                        
            await _unitOfWork.CommitTransactionAsync();
            
            return organisation.OrgId;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new Exception("Create Organisation and Organisation Admin failed.", ex);
        }
    }
}