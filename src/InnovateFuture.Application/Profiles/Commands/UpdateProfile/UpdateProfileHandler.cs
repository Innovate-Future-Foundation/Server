using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

namespace InnovateFuture.Application.Profiles.Commands.UpdateProfile;
public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Guid>
{
    private readonly IProfileRepository _profileRepository;
    public UpdateProfileHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<Guid> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
    { 
        // get existing Profile by id
        Profile profile = await _profileRepository.GetByIdAsync(command.ProfileId);
        
        // update
        profile.UpdateProfile(
            command.Name,
            command.Email,
            command.Phone,
            command.Avatar,
            command.IsActive
            );
        
        await _profileRepository.UpdateAsync();
        
        return profile.ProfileId;
    }
}

