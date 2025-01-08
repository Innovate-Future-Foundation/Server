using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using InnovateFuture.Application.Organisations.Queries.GetOrganisation;

namespace InnovateFuture.Api.Controllers.OrganisationsController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class OrganisationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public OrganisationsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Creates a new organisation.
    /// </summary>
    /// <param name="request">The details of the organisation to create.</param>
    /// <returns>A response containing the ID of the created organisation.</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateOrganisation([FromBody] CreateOrganisationRequest request)
    {
        var command = _mapper.Map<CreateOrganisationCommand>(request);
        var orgId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrganisation), new { id = orgId }, new { OrgId = orgId });
    }

    /// <summary>
    /// Gets an organisation by ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to get.</param>
    /// <returns>The organisation information.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetOrganisationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrganisation(Guid id)
    {
        var query = new GetOrganisationQuery { OrgId = id };
        var organisation = await _mediator.Send(query);
        
        if (organisation == null)
        {
            return NotFound();
        }

        var response = _mapper.Map<GetOrganisationResponse>(organisation);
        return Ok(response);
    }
}