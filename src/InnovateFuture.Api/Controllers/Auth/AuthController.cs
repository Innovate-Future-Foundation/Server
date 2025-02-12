using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Services.Auth.ConfirmEmail;
using Microsoft.AspNetCore.Mvc;
using MediatR;


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
}