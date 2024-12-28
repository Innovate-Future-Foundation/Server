using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrgRepository _orgRepository;
    private readonly IProfileRepository _profileRepository;
    public CreateUserHandler(IUserRepository userRepository,IRoleRepository roleRepository,IOrgRepository orgRepository,IProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _orgRepository = orgRepository;
        _profileRepository = profileRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Validate OrgId, RoleId 
        var role = await _roleRepository.GetByIdAsync(command.RoleId);
        var organisation = await _orgRepository.GetByIdAsync(command.OrgId);
        
        // Create user
        var user = new User(
            command.CognitoUuid,
            command.Email
            );
        // Add other details if provided
        user.UpdateUserDetails(
            null,
            command.Email,
            command.GivenName,
            command.FamilyName,
            command.Phone,
            command.Birthday
            );

        Profile invitedByProfile = null;
        Profile supervisedByProfile = null;
        
        if (command.InvitedBy.HasValue)
        {
            invitedByProfile = await _profileRepository.GetByIdAsync(command.InvitedBy.Value);
        }
        else if (command.SupervisedBy.HasValue)
        {
            supervisedByProfile = await _profileRepository.GetByIdAsync(command.SupervisedBy.Value);
        }
        
        var profile = new Profile(
            user,
            role,
            organisation,
            invitedByProfile,
            supervisedByProfile
        );
        
        
        // Add profile to user's navigation property
        user.AddProfile(profile);
        
        // Fill profile id
        user.UpdateDefaultProfile(profile.ProfileId);
        
        await _userRepository.AddAsync(user); // This will make sure user and its profile be created at the same time

        return user.UserId;
    }
}

