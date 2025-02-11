using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Controllers.OrganisationsController;
using InnovateFuture.Application.Organisations.Queries.GetOrganisation;
using InnovateFuture.Application.Services.Auth.ConfirmEmail;
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
    /// User click "Verify email" button
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("email-verification")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        try
        {
            var command = _mapper.Map<ConfirmEmailCommand>(request);
            bool success = await _mediator.Send(command);

            if (!success)
            {
                return BadRequest(new { Message = "Email confirmation failed" });
            }
            return Ok("Email confirmation successful");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Debug ERROR] Email Verification Failed: {ex.Message}");
            return StatusCode(500, new { Message = "An error occurred during email verification.", Details = ex.Message });
        }
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
        if (registerResult == null)
        {
            return BadRequest(new { Message = "Register organisation admin failed" });
        }
        
        var (profileId, user) = registerResult.Value;
        // 2⃣️ Send Email in the Background (Non-blocking)
        var sendVerificationEmailCommand = new SendVerificationEmailCommand(user, profileId);;
        _ = Task.Run(async () => await _mediator.Send(sendVerificationEmailCommand));
       
        return Ok("Register organisation admin and send email successful");
    }
}