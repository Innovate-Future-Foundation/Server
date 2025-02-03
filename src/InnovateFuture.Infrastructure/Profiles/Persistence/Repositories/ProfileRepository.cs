using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Repositories;

public class ProfileRepository:IProfileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProfileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Profile> GetByIdAsync(Guid id)
    {
        var profile = await _dbContext.Profiles
            .Include(p => p.User)
            .Include(p=>p.Organisation)
            .Include(p=>p.Role)
            .Include(p=>p.InviterProfile)
            .Include(p=>p.SupervisorProfile)
            .FirstOrDefaultAsync(p=>p.ProfileId == id);
        if (profile == null)

        {
            throw new IFEntityNotFoundException("Profile",id);
        }
        return profile;
    }

    public async Task AddAsync(Profile profile)
    {
        await _dbContext.Profiles.AddAsync(profile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Profile> Data, int TotalItems)> GetPagedAsync(
        Expression<Func<Profile, bool>>? predicate = null,
        int? limit = null,
        int offset = 0,
        string? orderBy = null)
    {
        IQueryable<Profile> query = _dbContext.Profiles
            .Include(p => p.User)
            .Include(p => p.Organisation)
            .Include(p => p.Role)
            .Include(p => p.InviterProfile)
            .Include(p => p.SupervisorProfile);


        if (predicate != null)
            query = query.Where(predicate);

        var totalItems = await query.CountAsync();

        if (!string.IsNullOrEmpty(orderBy))
        {
            var parts = orderBy.Split(' ');
            var propertyName = parts[0];
            var isAscending = parts.Length == 1 || parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase);

            query = isAscending 
                ? query.OrderBy(p => EF.Property<object>(p, propertyName))
                : query.OrderByDescending(p => EF.Property<object>(p, propertyName));
        }

        var data = await query
            .Skip(offset)
            .Take(limit ?? 10)
            .ToListAsync();

        return (data, totalItems);
    }

}