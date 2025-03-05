using InnovateFuture.Application.Services.UserService;
using MediatR;

namespace InnovateFuture.Application.Auth.Commands.Password;

public class ResetPasswordHandler: IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserService _userService;

    public ResetPasswordHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        // 1⃣️ get user by email
        var user = await _userService.GetUserByEmailAsync(command.Email, cancellationToken);
        
        // 2⃣️ reset password
        await _userService.ResetPasswordAsync(user, command.NewPassword, command.ResetPasswordToken,  cancellationToken);
        
        return true;
    }
}