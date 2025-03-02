using InnovateFuture.Application.Services.UserService;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ResetPasswordHandler: IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUserService _userService;

    public ResetPasswordHandler(IProfileRepository profileRepository, IUserService userService)
    {
        _profileRepository = profileRepository;
        _userService = userService;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ get user by profileId
        var user = await _profileRepository.GetUserByProfileId(command.ProfileId, cancellationToken);
        
        // 2⃣️ reset password
        await _userService.ResetPasswordAsync(user, command.NewPassword, cancellationToken);
        
        return true;
    }
}