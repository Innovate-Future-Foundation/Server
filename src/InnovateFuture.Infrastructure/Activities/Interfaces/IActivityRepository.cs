
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Activities.Interfaces;

public interface IActivityRepository
{
    Task<DomainActivity> AddAsync(DomainActivity activity);
    Task<DomainActivity> GetByIdAsync(long activityId);
    Task<IEnumerable<DomainActivity>> GetAllAsync();
    Task UpdateAsync(DomainActivity activity);
    Task DeleteAsync(DomainActivity activity);
}