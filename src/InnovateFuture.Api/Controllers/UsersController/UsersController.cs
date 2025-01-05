using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InnovateFuture.Api.Controllers.UsersController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]

public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public UsersController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The details of the user to create.</param>
    /// <returns>A response containing the ID of the created user.</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUser),new{id = userId},new { UserId = userId });
    }
    
    /// <summary>
    /// Updates user details by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The updated user details.</param>
    /// <returns>A response containing the ID of the updated user.</returns>
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        command.UserId = id;
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }

    /// <summary>
    /// Retrieves a user by its specified ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The details of the specified user.</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var query = new GetUserQuery { UserId = id };
        var user = await _mediator.Send(query);
        var userResponse =  _mapper.Map<GetUserResponse>(user);
        return Ok(userResponse);
    }
    
    /// <summary>
    /// Retrieves a list of users based on specified query parameters.
    /// </summary>
    /// <param name="queries">The query parameters to filter users.</param>
    /// <returns>A list of users that match the query parameters.</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] QueryUsersRequest queries)
    {
        var query = _mapper.Map<GetUsersQuery>(queries);
        var users = await _mediator.Send(query);
        var usersResponse = _mapper.Map<IEnumerable<GetUserResponse>>(users);
        return Ok(usersResponse);
    }
}