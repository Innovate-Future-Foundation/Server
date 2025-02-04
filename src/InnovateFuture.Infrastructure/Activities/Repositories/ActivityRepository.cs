using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Interfaces;
using InnovateFuture.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Activities.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DomainActivity> AddAsync(DomainActivity activity)
    {
        // 添加并发检查
        var entry = await _context.Activities.AddAsync(activity);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // 处理并发冲突
            throw;
        }
        return entry.Entity;
    }

    public async Task<DomainActivity> GetByIdAsync(long activityId)
    {
        // 包含相关实体
        return await _context.Activities
            // .Include(a => a.Day) // 添加对Day实体的包含
            .Include(a => a.Template)
            .Include(a => a.ActivityLeadProfile)
            .Include(a => a.Organisation)
            .FirstOrDefaultAsync(a => a.ActivityId == activityId);
    }

    public async Task<(IEnumerable<DomainActivity> Activities, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        ActivityFilterModel filter)
    {
        var query = _context.Activities
            // .Include(a => a.Day) // 添加对Day实体的包含
            .Include(a => a.Template)
            .AsQueryable();

        // 应用筛选条件
        if (filter != null)
        {
            if (filter.OrgId.HasValue)
                query = query.Where(a => a.OrgId == filter.OrgId);
                
            if (filter.DayId.HasValue)
                query = query.Where(a => a.DayId == filter.DayId);
                
            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(a => a.Title.Contains(filter.Title));
                
            if (filter.StartTime.HasValue)
                query = query.Where(a => a.StartTime >= filter.StartTime);
                
            if (filter.EndTime.HasValue)
                query = query.Where(a => a.EndTime <= filter.EndTime);
                
            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(a => a.Status == filter.Status);
        }

        var totalCount = await query.CountAsync();
        
        var activities = await query
            .OrderByDescending(a => a.StartTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (activities, totalCount);
    }

    public async Task UpdateAsync(DomainActivity activity)
    {
        // 添加乐观并发控制
        _context.Entry(activity).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ActivityExists(activity.ActivityId))
                throw new Exception($"Activity with ID {activity.ActivityId} not found");
            throw;
        }
    }

    private async Task<bool> ActivityExists(long activityId)
    {
        return await _context.Activities.AnyAsync(a => a.ActivityId == activityId);
    }

    // 实现DeleteAsync方法
    public async Task DeleteAsync(DomainActivity activity)
    {
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
    }

    // 实现GetAllAsync方法
    public async Task<IEnumerable<DomainActivity>> GetAllAsync()
    {
        return await _context.Activities.ToListAsync();
    }
}