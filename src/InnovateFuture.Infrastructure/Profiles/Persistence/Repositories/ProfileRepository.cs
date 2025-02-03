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
            .Include(p=>p.InvitedByProfile)
            .Include(p=>p.SupervisedByProfile)
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
            .Include(p => p.InvitedByProfile)
            .Include(p => p.SupervisedByProfile);

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

    public async Task<(List<Profile> Data, string? NextCursor)> GetWithCursorAsync(
        Expression<Func<Profile, bool>>? predicate = null,
        int? pageSize = null,
        string? cursor = null,
        string? orderBy = null)
    {
        IQueryable<Profile> query = _dbContext.Profiles
            .Include(p => p.User)
            .Include(p => p.Organisation)
            .Include(p => p.Role)
            .Include(p => p.InvitedByProfile)
            .Include(p => p.SupervisedByProfile);

        if (predicate != null)
            query = query.Where(predicate);

        if (!string.IsNullOrEmpty(cursor))
        {
            var decodedCursor = Convert.FromBase64String(cursor);
            var cursorValue = System.Text.Encoding.UTF8.GetString(decodedCursor);
            query = query.Where(p => p.CreatedAt > DateTime.Parse(cursorValue));
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            var parts = orderBy.Split(' ');
            var propertyName = parts[0];
            var isAscending = parts.Length == 1 || parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase);

            query = isAscending 
                ? query.OrderBy(p => EF.Property<object>(p, propertyName))
                : query.OrderByDescending(p => EF.Property<object>(p, propertyName));
        }
        else
        {
            query = query.OrderBy(p => p.CreatedAt);
        }

        var data = await query
            .Take(pageSize ?? 10)
            .ToListAsync();

        string? nextCursor = null;
        if (data.Count == pageSize)
        {
            var lastItem = data.Last();
            var cursorBytes = System.Text.Encoding.UTF8.GetBytes(lastItem.CreatedAt.ToString());
            nextCursor = Convert.ToBase64String(cursorBytes);
        }

        return (data, nextCursor);
    }
}