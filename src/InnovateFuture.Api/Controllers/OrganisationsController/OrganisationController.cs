using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

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

    // 暂时需要这个方法因为 CreatedAtAction 引用了它
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganisation(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}