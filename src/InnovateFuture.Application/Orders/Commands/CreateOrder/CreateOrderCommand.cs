using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InnovateFuture.Application.Orders.Commands.CreateOrder;
public class CreateOrderCommand : IRequest<Guid>
{
    public string CustomerName { get; set; }
    [Required]
    public List<CreateOrderItem> Items { get; set; }

    public class CreateOrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

