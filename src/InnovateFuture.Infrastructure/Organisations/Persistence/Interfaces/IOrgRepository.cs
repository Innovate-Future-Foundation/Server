using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

public interface IOrgRepository
{
    Task<Organisation> GetByIdAsync(Guid id);
    Task AddAsync(Organisation order);
    Task UpdateAsync();
    Task GetAllAsync(List<Organisation> organisations);
}