using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrgRepository _orgRepository;
    private readonly IProfileRepository _profileRepository;
    public CreateUserHandler(IUserRepository userRepository, IOrgRepository orgRepository,IProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _orgRepository = orgRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Validate OrgId 
        var organisation = await _orgRepository.GetByIdAsync(command.OrgId);
        
        // Create user
        var user = new User(command.Email);

        Profile? inviterProfile = null;
        Profile? supervisorProfile = null;

        if (command.Inviter.HasValue)
        {
            inviterProfile = await _profileRepository.GetByIdAsync(command.Inviter.Value);
        }

        if (command.Supervisor.HasValue)
        {
            supervisorProfile = await _profileRepository.GetByIdAsync(command.Supervisor.Value);
        }
        
        var profile = new Profile(
            user.UserId,
            command.RoleEnum,
            organisation.OrgId,
            inviterProfile?.ProfileId,
            supervisorProfile?.ProfileId
        );
        
        profile.AddUser(user);
        profile.AddOrganisation(organisation);
        profile.AddInviterProfile(inviterProfile);
        profile.AddSupervisorProfile(supervisorProfile);
        
        // Add profile to user's navigation property
        user.AddProfile(profile);
        
        // Fill profile id
        user.UpdateDefaultProfile(profile.ProfileId);
        
        await _userRepository.AddAsync(user); // This will make sure user and its profile be created at the same time

        return user.UserId;
    }
}

