using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;

namespace InnovateFuture.Application.Orders.Commands.CreateOrder;
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    public CreateOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order(command.CustomerName);

        foreach (var item in command.Items)
        {
            order.AddItem(new OrderItem(item.ProductName, item.Quantity,item.UnitPrice));
        }
        await _orderRepository.AddAsync(order);

        return order.Id;
    }
}

