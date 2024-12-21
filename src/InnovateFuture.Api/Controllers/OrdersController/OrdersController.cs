using AutoMapper;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Controllers.OrderController;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
using InnovateFuture.Application.Orders.Queries.GetOrder;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace InnovateFuture.Api.Controllers.OrdersController;

[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersion.V1))]
[ApiController]
[Route("api/v1/[controller]")]

public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrdersController(IMediator mediator,IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var command = _mapper.Map<CreateOrderCommand>(request);
        var orderId = await _mediator.Send(command);
        return Ok(new { OrderId = orderId });
    }

    /// <summary>
    /// Retrieves an order by its specified ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var query = new GetOrderQuery { OrderId = id };
        var order = await _mediator.Send(query);
        var orderResponse =  _mapper.Map<GetOrderResponse>(order);
        return Ok(orderResponse);
    }
}
