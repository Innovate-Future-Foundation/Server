using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Activities.Persistence.Repositories;

public class ActivityRepository:IActivityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Activity activity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Activities.AddAsync(activity, cancellationToken);
    }
    
    public async Task<Activity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var activity = await _dbContext.Activities
            .Include(a=>a.DaysBelong)
            .Include(a=>a.TeachersAssigned)
            .FirstOrDefaultAsync(o=>o.Id == id);
        if (activity == null)
        {
            throw new IFEntityNotFoundException("Activity", id);
        }
        return activity;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Activity> data, int totalItems)> GetAnyAsync(Expression<Func<Activity, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<Activity> query = _dbContext.Activities;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var activities = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (activities, totalItems);
    }
}