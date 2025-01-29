using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

public interface IOrgRepository
{
    Task<Organisation> GetByIdAsync(Guid id);
    Task AddAsync(Organisation organisation);
    Task UpdateAsync();

    Task<(List<Organisation> data, int totalItems)> GetAnyAsync(
        Expression<Func<Organisation, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}