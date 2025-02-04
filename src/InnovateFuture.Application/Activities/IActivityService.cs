using InnovateFuture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnovateFuture.Application.Activities;

public interface IActivityService
{
    Task<DomainActivity> CreateActivityAsync(DomainActivity domainActivity);
    Task<DomainActivity> GetActivityByIdAsync(long activityId);
    Task<IEnumerable<DomainActivity>> GetAllActivitiesAsync();
    Task<IEnumerable<DomainActivity>> GetAllActivitiesByOrgIdAsync(long orgId); // 新增方法
    Task UpdateActivityAsync(DomainActivity domainActivity);
    Task DeleteActivityAsync(long activityId);
}