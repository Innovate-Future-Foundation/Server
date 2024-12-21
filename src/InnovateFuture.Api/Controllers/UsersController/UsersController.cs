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
    /// Create a new User.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }
    
    /// <summary>
    /// Update user details by its specified ID.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        var userId = await _mediator.Send(command);
        return Ok(new { UserId = userId });
    }

    /// <summary>
    /// Retrieves a user by its specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// Retrieves users by specified queries.
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] QueryUsersRequest? queries)
    {
        var query = _mapper.Map<GetUsersQuery>(queries);
        var users = await _mediator.Send(query);
        var usersResponse = _mapper.Map<IEnumerable<GetUserResponse>>(users);
        return Ok(usersResponse);
    }
}