using InnovateFuture.Domain.Exceptions;

namespace InnovateFuture.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; } 
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    public OrderItem(string productName, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new IFDomainValidationException("Product name is required.");

        if (quantity <= 0)
            throw new IFDomainValidationException("Quantity must be greater than zero.");

        if (unitPrice <= 0)
            throw new IFDomainValidationException("Unit price must be greater than zero.");

        if (quantity * unitPrice > 1000)
            throw new IFBusinessRuleViolationException("Total price for a single item cannot exceed $1000.");

        if (RestrictedProducts.Contains(productName))
            throw new IFBusinessRuleViolationException($"The product '{productName}' is restricted and cannot be ordered.");

        Id = Guid.NewGuid();
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    private static readonly HashSet<string> RestrictedProducts = new HashSet<string>
    {
        "RestrictedItem1",
        "RestrictedItem2"
    };
    
}