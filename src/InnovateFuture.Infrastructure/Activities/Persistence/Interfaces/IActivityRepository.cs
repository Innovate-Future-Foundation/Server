using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;

public interface IActivityRepository
{
    Task AddAsync(Activity activity, CancellationToken cancellationToken = default);
    Task<Activity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<Activity> data, int totalItems)> GetAnyAsync(
        Expression<Func<Activity, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}