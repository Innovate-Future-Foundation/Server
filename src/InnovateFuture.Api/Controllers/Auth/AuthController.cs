using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Auth.Commands.ConfirmEmail;
using InnovateFuture.Application.Auth.Commands.Login;
using InnovateFuture.Application.Auth.Commands.Password;
using InnovateFuture.Application.Auth.Commands.Register;
using InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;
using InnovateFuture.Application.Auth.Commands.SendTemporaryPassword;
using InnovateFuture.Application.Auth.Commands.SendVerificationEmail;
using InnovateFuture.Application.Auth.Queries.GetMe;
using InnovateFuture.Application.Auth.SendTemporaryPasswordEmail;
using InnovateFuture.Application.Services.Auth.Register;
using InnovateFuture.Application.Services.Security.TokenService;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

namespace InnovateFuture.Api.Controllers.Auth;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly JWTConfig _jwtConfig;

    public AuthController(IMediator mediator, IMapper mapper, IOptions<JWTConfig> jwtOptions)
    {
        _mediator = mediator;
        _mapper = mapper;
        _jwtConfig = jwtOptions.Value;
    }
    
    /// <summary>
    /// Register organisation admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("register-organisation-admin")]
    public async Task<IActionResult> RegisterOrganisationAdmin([FromBody] RegisterOrganisationAdminRequest request)
    {
        // 1⃣️ Update tables 
        var registerCommand = _mapper.Map<RegisterOrganisationAdminCommand>(request);
        var registerResult = await _mediator.Send(registerCommand);
        
        var (profileId, user, token) = registerResult.Value;
        // 2⃣️ Send Email in the Background (Non-blocking)
        var sendVerificationEmailCommand = new SendVerificationEmailCommand(user, profileId, token, "email-verification");
        _ = Task.Run(async () => await _mediator.Send(sendVerificationEmailCommand));

        return Ok("Register organisation admin and send email successful");
    }

    
    /// <summary>
    /// User click "Verify email" button
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("email-verification")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var command = _mapper.Map<ConfirmEmailCommand>(request);
        var confirmResult = await _mediator.Send(command);
        
        var (email, result, isAdmin) = confirmResult;

        if (isAdmin)
        {
            // Store token in HTTP-only Cookie
            Response.Cookies.Append("access-token", result, new CookieOptions
            {
                HttpOnly = true, // prevent JS access
                Secure = true,   // use https 
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(3),
                Domain = _jwtConfig.Domain
            });
        }
        else
        {
            var sendTempPasswordCommand = new SendTemporaryPasswordCommand(email, result);
            _ = Task.Run(async () => await _mediator.Send(sendTempPasswordCommand));
        }
        return Ok("Email verification successful!");
    }

    [AllowAnonymous]
    [HttpPost("resend-verification-email")]
    public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerficationEmailRequest request)
    {
        var command = _mapper.Map<ResendVerificationEmailCommand>(request);
        await _mediator.Send(command);
        return Ok("Resend email verification successful!");
    }
    
    /// <summary>
    /// User login by email and password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = _mapper.Map<LoginCommand>(request);
        var accessToken = await _mediator.Send(command);
        
        // Store token in HTTP-only Cookie
        Response.Cookies.Append("access-token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(3),
            Domain = _jwtConfig.Domain
        });
        return Ok("Login successful!");
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var profileIdClaim = User.FindFirst("ProfileId")?.Value;
        if (string.IsNullOrEmpty(profileIdClaim))
        {
            return Unauthorized();
        }

        if (!Guid.TryParse(profileIdClaim, out var profileId))
        {
            return BadRequest("failed to parse profile Id");
        }
        var profile = await _mediator.Send(new GetMeQuery(profileId));
        var getMeResponse = _mapper.Map<GetMeResponse>(profile);
        
        return Ok(getMeResponse);
    }

    /// <summary>
    /// user reset password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var resetPasswordCommand = _mapper.Map<ResetPasswordCommand>(request);
        
        var success = await _mediator.Send(resetPasswordCommand);
        if (!success)
        {
            return BadRequest("failed to reset password.");
        }
        return Ok("Password reset successful, please login again.");
    }

    /// <summary>
    /// only need user email
    /// verification successful then need go to reset-password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var command = _mapper.Map<ForgotPasswordCommand>(request);
        var result = await _mediator.Send(command);
        if (!result)
        {
            return BadRequest("failed to reset password.");
        }
        
        return Ok("Password reset link has been sent to your email.");
    }
    
    /// <summary>
    /// user logout
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access-token");
        return Task.FromResult<IActionResult>(Ok("Logout successful"));
    }
    
    /// <summary>
    /// Register normal user(invited by admin)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("invite")]
    public async Task<IActionResult> RegisterNormalUser([FromBody] RegisterNormalUserRequest request)
    {
        var profileIdClaims = User.FindFirst("ProfileId")?.Value;
        if (string.IsNullOrEmpty(profileIdClaims) || !Guid.TryParse(profileIdClaims, out var profileId))
        {
            return Unauthorized();
        }
        
        var registerCommand = _mapper.Map<RegisterNormalUserCommand>(request);
        registerCommand.InviterProfileId = profileId;
        
        var registerResult = await _mediator.Send(registerCommand);
        var (userProfileId, user, token, roleEnum) = registerResult.Value;
        // Send Email in the Background (Non-blocking)
        var sendVerificationEmailCommand = new SendVerificationEmailCommand(user, userProfileId, token, "email-verification", roleEnum);
        _ = Task.Run(async () => await _mediator.Send(sendVerificationEmailCommand));

        return Ok("Invite user successful, please let user check email and confirm.");
    }
}