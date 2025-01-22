using Microsoft.EntityFrameworkCore;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Orders.Persistence.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        var order= await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            throw new IFEntityNotFoundException("Order", id);
        }
        return order;
    }
    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }
}