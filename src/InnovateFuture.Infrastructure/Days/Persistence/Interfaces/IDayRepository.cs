using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Days.Persistence.Interfaces;

public interface IDayRepository
{
    Task AddAsync(Day day, CancellationToken cancellationToken = default);
    Task<Day> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<Day> data, int totalItems)> GetAnyAsync(
        Expression<Func<Day, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}