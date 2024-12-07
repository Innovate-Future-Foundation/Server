using InnovateFuture.Api.Enums;
using InnovateFuture.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using InnovateFuture.Application.Orders.Commands;
using InnovateFuture.Application.Orders.Queries;

namespace InnovateFuture.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(APIVersion.V1))]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return Ok(new { OrderId = orderId });
        }

        /// <summary>
        /// Retrieves an order by its specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var query = new GetOrderQuery { OrderId = id };
            var order = await _mediator.Send(query);

            if (order == null)
            {
                throw new NotFoundException("this order does not exist!");
            }
            return Ok(order);
        }
    }
}