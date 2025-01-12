using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Roles.Queries.GetRole;
using InnovateFuture.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using InnovateFuture.Application.Services;

namespace InnovateFuture.Api.Controllers.RolesController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]

public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IAccessControlService _accessControlService;
    public RolesController(IMediator mediator,IMapper mapper, IAccessControlService accessControlService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _accessControlService = accessControlService;
    }

    /// <summary>
    /// Retrieves a Role by its specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var query = new GetRoleQuery { RoleId = id };
        var role = await _mediator.Send(query);
        var roleResponse = _mapper.Map<GetRoleResponse>(role);
        return Ok(roleResponse);
    }
    

    /// <summary>
    /// Retrieves a Roles by name/code name or all Roles.
    /// </summary>
    /// <param name="queries"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetRoles([FromQuery]QueryRolesRequest queries)
    {
        var query = _mapper.Map<GetRolesQuery>(queries);
        var roles = await _mediator.Send(query);
        var rolesResponse = _mapper.Map<IEnumerable<GetRoleResponse>>(roles);
        return Ok(rolesResponse);
    }


    /// <summary>
    /// Retrieves permissions for the specified role.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{roleId}/permissions")]
    public async Task<IActionResult> GetPermissions(Guid roleId)
    {
        try
        {
            var permissions = await _accessControlService.GetPermissions(roleId);
            return Ok(permissions);
        }
        catch (Exception ex)
        {
            // Log exception if necessary, and return an error response
            return NotFound(new { message = ex.Message });
        }
    }
}