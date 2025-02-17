using InnovateFuture.Application.Services.Auth.TokenService;
using InnovateFuture.Application.Services.Auth.UserService;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Exceptions;
using InnovateFuture.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Application.Services.Auth.Login;

public class LoginHandler: IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public LoginHandler(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // check user exist
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null) 
        {
            throw new IFEntityNotFoundException("User", command.Email);
        };

        // check user is confirmed
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
        {
            throw new IFBusinessRuleViolationException("Please confirm your email before logging in.");
        }

        // check user password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!isPasswordValid)
        {
            throw new IFUnauthorizedActionException("Invalid password or email.");
        }

        // generate access token
        var accessToken = await _tokenService.GenerateJwtTokenAsync(user.DefaultProfileId ?? throw new Exception("No Default Profile Id"));
        return accessToken;
    }
}