using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Commands.CreateUser;
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IOrgRepository _orgRepository;

    public CreateUserHandler(IUserRepository userRepository,IRoleRepository roleRepository,IOrgRepository orgRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _orgRepository = orgRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Validate OrgId, RoleId 
        await _roleRepository.GetByIdAsync(command.RoleId);
        await _orgRepository.GetByIdAsync(command.OrgId);
        
        // Create user
        var user = new User(
            command.CognitoUuid,
            command.Email
            );
        
        // Create profile
        var profile = new Profile(
            user.UserId,
            command.OrgId,
            command.RoleId,
            command.InvitedByProfile,
            command.SupervisedByProfile
            );
        
        // Add profile to user's navigation property
        user.AddProfile(profile);
        
        // Fill profile id
        user.UpdateDefaultProfile(profile.ProfileId);
        
        await _userRepository.AddAsync(user); // This will make sure user and its profile be created at the same time

        return user.UserId;
    }
}

