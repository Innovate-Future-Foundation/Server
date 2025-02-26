using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Auth.ConfirmEmail;
using InnovateFuture.Application.Auth.Login;
using InnovateFuture.Application.Auth.Register;
using InnovateFuture.Application.Auth.SendVerificationEmail;
using InnovateFuture.Application.Services.Security.TokenService;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        
        var (profileId, user) = registerResult.Value;
        // 2⃣️ Send Email in the Background (Non-blocking)
        var sendVerificationEmailCommand = new SendVerificationEmailCommand(user, profileId);;
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
        return Ok("Email verification successful!");
    }

    //TODO: @xiaobao
    // [HttpGet("get-me")]
    // public async Task<IActionResult> GetMe()
    // {
    //     
    // }
    
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
}