using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using InnovateFuture.Application.Profiles.Queries.GetProfiles;


namespace InnovateFuture.Api.Controllers.Profiles;

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
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateProfileRequest request)
    {
        var command = _mapper.Map<UpdateProfileCommand>(request);
        command.Id = id;
        var profileId = await _mediator.Send(command);
        return Ok(new {  profileId });
    }

    /// <summary>
    /// Retrieves a Profile by its specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id,[FromQuery] bool includeDetails=false)
    {
        var query = new GetProfileQuery { ProfileId = id };
        var profile = await _mediator.Send(query);
        
        var profileResponse =  includeDetails? (object)_mapper.Map<GetProfileWithDetailsResponse>(profile):
            _mapper.Map<GetProfileResponse>(profile);
        
        return Ok(profileResponse);
    }

    /// <summary>
    /// Retrieves a list of Profiles based on the specified query parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProfiles([FromQuery]QueryProfilesRequest request)
    {
        var query = _mapper.Map<GetProfilesQuery>(request);
        var paginatedProfiles = await _mediator.Send(query);
        
        var profilesResponse = request.IncludeDetails ?? false
            ? (object)_mapper.Map<GetProfileWithDetailsPaginatedResponse>(paginatedProfiles)
            : _mapper.Map<GetProfilePaginatedResponse>(paginatedProfiles);
        
        return Ok(profilesResponse);
    }
    
    [AllowAnonymous]
    [HttpGet("Permissions")]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = new
        {
            canEditOrganisationDetailForm = new[] { "PlatformAdmin", "OrgAdmin" },
            canViewManagerScrollList = new[] { "PlatformAdmin", "OrgAdmin" },
            needViewOrganisationOfUser = new[] { "PlatformAdmin" },
        };
        return Ok(permissions);
    }
}