using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Activities.Commands.CreateActivity;
using InnovateFuture.Application.Activities.Commands.UpdateActivity;
using InnovateFuture.Application.Activities.Queries.GetActivity;
using InnovateFuture.Application.Activities.Queries.GetActivities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnovateFuture.Api.Controllers.ActivitiesController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ActivitiesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new activity.
    /// </summary>
    /// <param name="request">The details of the activity to create.</param>
    /// <returns>A response containing the ID of the created activity.</returns>
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateActivity([FromBody] CreateActivityRequest request)
    {
        var command = _mapper.Map<CreateActivityCommand>(request);
        var activityId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetActivity), new { id = activityId }, new { ActivityId = activityId });
    }

    /// <summary>
    /// Retrieves a list of activities based on specified query parameters.
    /// </summary>
    /// <param name="request">The query parameters to filter activities.</param>
    /// <returns>A paginated list of activities that match the query parameters.</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(GetActivityPaginatedResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetActivityPaginatedResponse>> GetActivities([FromQuery] QueryActivityRequest request)
    {
        var query = _mapper.Map<GetActivitiesQuery>(request);
        var result = await _mediator.Send(query);
        return Ok(_mapper.Map<GetActivityPaginatedResponse>(result));
    }

    /// <summary>
    /// Retrieves an activity by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the activity to retrieve.</param>
    /// <returns>The details of the specified activity.</returns>
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetActivityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetActivityResponse>> GetActivity(Guid id)
    {
        var result = await _mediator.Send(new GetActivityQuery { ActivityId = id });
        return Ok(_mapper.Map<GetActivityResponse>(result));
    }

    /// <summary>
    /// Updates activity details by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the activity to update.</param>
    /// <param name="request">The updated activity details.</param>
    /// <returns>A response containing the ID of the updated activity.</returns>
    [AllowAnonymous]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> UpdateActivity(Guid id, [FromBody] UpdateActivityRequest request)
    {
        var command = _mapper.Map<UpdateActivityCommand>(request);
        command.Id = id;
        var activityId = await _mediator.Send(command);
        return Ok(new { ActivityId = activityId });
    }

    /// <summary>
    /// Assigns a teacher to an activity.
    /// </summary>
    /// <param name="id">The ID of the activity.</param>
    /// <param name="request">The teacher assignment details.</param>
    /// <returns>No content on successful assignment.</returns>
    [AllowAnonymous]
    [HttpPost("{id:guid}/teachers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AssignTeacher(Guid id, [FromBody] AssignTeacherRequest request)
    {
        await _mediator.Send(new AssignTeacherCommand { ActivityId = id, TeacherId = request.TeacherId });
        return NoContent();
    }

    /// <summary>
    /// Removes a teacher from an activity.
    /// </summary>
    /// <param name="id">The ID of the activity.</param>
    /// <param name="teacherId">The ID of the teacher to remove.</param>
    /// <returns>No content on successful removal.</returns>
    [AllowAnonymous]
    [HttpDelete("{id:guid}/teachers/{teacherId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveTeacher(Guid id, Guid teacherId)
    {
        await _mediator.Send(new RemoveTeacherCommand { ActivityId = id, TeacherId = teacherId });
        return NoContent();
    }
}