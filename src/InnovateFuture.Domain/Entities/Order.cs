using InnovateFuture.Domain.Exceptions;

namespace InnovateFuture.Domain.Entities;
    public class Order
    {
        public Guid Id { get; private set; }
        public String CustomerName { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);

        public Order(String customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new IFDomainValidationException("Customer name is required.");

            if (!customerName.StartsWith("VIP", StringComparison.OrdinalIgnoreCase))
                throw new IFBusinessRuleViolationException("Customer name must start with 'VIP' for priority orders.");
            
            Id = Guid.NewGuid();
            CustomerName = customerName;
            CreatedDate = DateTime.UtcNow;
        }

        public void AddItem(OrderItem item)
        {
            if (Items.Count >= 10)
                throw new IFBusinessRuleViolationException("An order cannot contain more than 10 items.");

            Items.Add(item);
        }
    }
