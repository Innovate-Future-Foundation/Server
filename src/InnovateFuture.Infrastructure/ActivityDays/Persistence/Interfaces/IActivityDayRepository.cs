using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.ActivityDays.Persistence.Interfaces;

public interface IActivityDayRepository
{
    Task AddAsync(ActivityDay activityDay, CancellationToken cancellationToken = default);
    Task<ActivityDay> GetByIdAsync(Guid activityId,Guid dayId, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<ActivityDay> data, int totalItems)> GetAnyAsync(
        Expression<Func<ActivityDay, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}