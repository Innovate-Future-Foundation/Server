using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Orders.Queries.GetOrder;

public class GetOrderQuery : IRequest<Order>
{
    public Guid OrderId { get; set; }
}


