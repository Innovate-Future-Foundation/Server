using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.Days.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Days.Persistence.Repositories;

public class DayRepository:IDayRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DayRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Day day, CancellationToken cancellationToken = default)
    {
        await _dbContext.Days.AddAsync(day, cancellationToken);
    }
    
    public async Task<Day> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var day = await _dbContext.Days
            .Include(a=>a.Activities)
            .FirstOrDefaultAsync(o=>o.Id == id);
       
        if (day == null)
        {
            throw new IFEntityNotFoundException("Day", id);
        }
        return day;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Day> data, int totalItems)> GetAnyAsync(Expression<Func<Day, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<Day> query = _dbContext.Days;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var days = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (days, totalItems);
    }
}