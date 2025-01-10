using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

public interface IOrgRepository
{
    Task<Organisation> GetByIdAsync(Guid id);
    Task AddAsync(Organisation organisation);
    Task UpdateAsync();
    Task<IEnumerable<Organisation>>GetAnyAsync(Expression<Func<Organisation, bool>> predicate);
}