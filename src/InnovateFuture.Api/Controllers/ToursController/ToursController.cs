using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Tours.Commands.CreateTour;
using InnovateFuture.Application.Tours.Commands.UpdateTour;
using InnovateFuture.Application.Tours.Queries.GetTour;
using InnovateFuture.Application.Tours.Queries.GetTours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovateFuture.Api.Controllers.ToursController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class ToursController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ToursController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new tour.
    /// </summary>
    /// <param name="request">The details of the tour to create.</param>
    /// <returns>A response containing the ID of the created tour.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateTour([FromBody] CreateTourRequest request)
    {
        var command = _mapper.Map<CreateTourCommand>(request);
        var tourId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTour), new { id = tourId }, new { TourId = tourId });
    }

    /// <summary>
    /// Retrieves a tour by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the tour to retrieve.</param>
    /// <returns>The details of the specified tour.</returns>
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetTourResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTourResponse>> GetTour(Guid id)
    {
        var result = await _mediator.Send(new GetTourQuery { TourId = id });
        return Ok(_mapper.Map<GetTourResponse>(result));
    }

    /// <summary>
    /// Updates tour details by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the tour to update.</param>
    /// <param name="request">The updated tour details.</param>
    /// <returns>A response containing the ID of the updated tour.</returns>
    [AllowAnonymous]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> UpdateTour(Guid id, [FromBody] UpdateTourRequest request)
    {
        var command = _mapper.Map<UpdateTourCommand>(request);
        command.Id = id;
        var tourId = await _mediator.Send(command);
        return Ok(new { TourId = tourId });
    }

    /// <summary>
    /// Retrieves a list of tours based on specified query parameters.
    /// </summary>
    /// <param name="request">The query parameters to filter tours.</param>
    /// <returns>A paginated list of tours that match the query parameters.</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(GetTourPaginatedResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetTourPaginatedResponse>> GetTours([FromQuery] QueryToursRequest request)
    {
        var query = _mapper.Map<GetToursQuery>(request);
        var result = await _mediator.Send(query);
        return Ok(_mapper.Map<GetTourPaginatedResponse>(result));
    }
}