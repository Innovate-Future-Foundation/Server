
namespace InnovateFuture.Api.Controllers.OrdersController;

public class CreateOrderRequest
{
    public string CustomerName { get; set; }
    
    public List<CreateOrderItem> Items { get; set; }

    public class CreateOrderItem
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}