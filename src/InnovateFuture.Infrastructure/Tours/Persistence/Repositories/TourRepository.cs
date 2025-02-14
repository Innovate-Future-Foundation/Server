using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Tours.Persistence.Repositories;

public class TourRepository:ITourRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TourRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tours.AddAsync(tour, cancellationToken);
    }
    
    public async Task<Tour> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tour = await _dbContext.Tours
            .Include(a=>a.Days)
            .Include(a=>a.EnrolledStudents)
            .Include(a=>a.LeaderProfile)
            .FirstOrDefaultAsync(o=>o.Id == id);
       
        if (tour == null)
        {
            throw new IFEntityNotFoundException("Tour", id);
        }
        return tour;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Tour> data, int totalItems)> GetAnyAsync(Expression<Func<Tour, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<Tour> query = _dbContext.Tours;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var tours = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (tours, totalItems);
    }
}