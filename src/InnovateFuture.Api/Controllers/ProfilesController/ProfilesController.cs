using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InnovateFuture.Api.Controllers.ProfilesController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]

public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public ProfilesController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Update Profile details by its specified ID.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var command = _mapper.Map<UpdateProfileCommand>(request);
        var ProfileId = await _mediator.Send(command);
        return Ok(new { ProfileId = ProfileId });
    }

    /// <summary>
    /// Retrieves a Profile by its specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var query = new GetProfileQuery { ProfileId = id };
        var profile = await _mediator.Send(query);
        var profileResponse =  _mapper.Map<GetProfileResponse>(profile);
        return Ok(profileResponse);
    }
}