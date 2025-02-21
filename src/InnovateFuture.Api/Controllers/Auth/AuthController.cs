using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Services.Auth.ConfirmEmail;
using InnovateFuture.Application.Services.Auth.Login;
using InnovateFuture.Application.Services.Auth.Register;
using InnovateFuture.Application.Services.Auth.SendVerificationEmail;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace InnovateFuture.Api.Controllers.Auth;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
            HttpOnly = true, // prevent JS access
            Secure = true,   // use https 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(3)
        });
        // TODO: Return dashboard page
        return Redirect("http://frontend/dashboard");
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
            HttpOnly = true, // prevent JS access
            Secure = true,   // use https 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(3)
        });
        return Ok("login successful");
    }
    
    /// <summary>
    /// user logout
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access-token");
        return Ok("logout successful");
        // TODO
        // return Redirect("http://frontend/dashboard");
    }
}