using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.ActivityProfiles.Persistence.Interfaces;

public interface IActivityProfileRepository
{
    Task AddAsync(ActivityProfile activityProfile, CancellationToken cancellationToken = default);
    Task<ActivityProfile> GetByIdAsync(Guid activityId,Guid profileId, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<ActivityProfile> data, int totalItems)> GetAnyAsync(
        Expression<Func<ActivityProfile, bool>>? predicate=null, int? limit = null, int offset=0, string? queryOrderBy=null);
}