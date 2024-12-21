namespace InnovateFuture.Api.Controllers.OrderController;
public class GetOrderResponse
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; }
}
