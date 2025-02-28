using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.ActivityDays.Persistence.Interfaces;
using InnovateFuture.Infrastructure.ActivityProfiles.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.ActivityProfiles.Persistence.Repositories;

public class ActivityProfileRepository:IActivityProfileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivityProfileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(ActivityProfile activityProfile, CancellationToken cancellationToken = default)
    {
        await _dbContext.ActivityProfiles.AddAsync(activityProfile, cancellationToken);
    }
    
    public async Task<ActivityProfile> GetByIdAsync(Guid activityId,Guid profileId, CancellationToken cancellationToken = default)
    {
        var activityProfiles = await _dbContext.ActivityProfiles
            .Include(a=>a.Teacher)
            .Include(a=>a.Activity)
            .FirstOrDefaultAsync(a=>a.AssignedActivitiesId == activityId && a.TeachersAssignedId == profileId);
        
        if (activityProfiles == null)
        {
            throw new IFEntityNotFoundException("ActivityProfile", $"{activityId} & {profileId}");
        }
        return activityProfiles;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<ActivityProfile> data, int totalItems)> GetAnyAsync(Expression<Func<ActivityProfile, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<ActivityProfile> query = _dbContext.ActivityProfiles;
        
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var activityProfiles = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (activityProfiles, totalItems);
    }
}