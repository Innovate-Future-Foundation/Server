using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.ActivityDays.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.ActivityDays.Persistence.Repositories;

public class ActivityDayRepository:IActivityDayRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivityDayRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(ActivityDay activityDay, CancellationToken cancellationToken = default)
    {
        await _dbContext.ActivityDays.AddAsync(activityDay, cancellationToken);
    }
    
    public async Task<ActivityDay> GetByIdAsync(Guid activityId,Guid dayId, CancellationToken cancellationToken = default)
    {
        var activityDays = await _dbContext.ActivityDays
            .Include(a=>a.Day)
            .Include(a=>a.Activity)
            .FirstOrDefaultAsync(o=>o.ActivitiesId == activityId && o.DaysBelongId == dayId);
        if (activityDays == null)
        {
            throw new IFEntityNotFoundException("Activity", $"{activityId} & {dayId}");
        }
        return activityDays;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<ActivityDay> data, int totalItems)> GetAnyAsync(Expression<Func<ActivityDay, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<ActivityDay> query = _dbContext.ActivityDays;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var activityDays = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (activityDays, totalItems);
    }
}