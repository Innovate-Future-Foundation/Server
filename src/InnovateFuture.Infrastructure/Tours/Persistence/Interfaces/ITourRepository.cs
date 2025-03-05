using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;

public interface ITourRepository
{
    Task AddAsync(Tour tour, CancellationToken cancellationToken = default);
    Task<Tour> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<Tour> data, int totalItems)> GetAnyAsync(
        Expression<Func<Tour, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}