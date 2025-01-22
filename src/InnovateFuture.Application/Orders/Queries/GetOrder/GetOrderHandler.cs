using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;

namespace InnovateFuture.Application.Orders.Queries.GetOrder;
public class GetOrderHandler : IRequestHandler<GetOrderQuery, Order>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        return order;
    }
}

