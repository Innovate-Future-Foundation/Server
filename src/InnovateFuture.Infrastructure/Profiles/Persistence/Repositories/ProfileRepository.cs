using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.VisualBasic.CompilerServices;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Repositories;

public class ProfileRepository:IProfileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProfileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Profile profile, CancellationToken cancellationToken = default)
    {
        await _dbContext.Profiles.AddAsync(profile, cancellationToken);
    }

    public async Task CheckProfileExistByUserIdOrgIdRoleAsync(Guid userId, Guid orgId, RoleEnum role, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Where(p => p.UserId == userId && p.OrgId == orgId && p.Role == role)
            .FirstOrDefaultAsync(cancellationToken);
        if (profile != null)
        {
            throw new IFConcurrencyException("Profile already exists.");
        }
    }

    public async Task<Profile> GetUserByProfileId(Guid profileId, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (profile == null)
        {
            throw new IFEntityNotFoundException("Profile",profileId);
        }
        return profile;
    }

    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Include(p => p.User)
            .Include(p=>p.Organisation)
            .Include(p=>p.InviterProfile)
            .Include(p=>p.SupervisorProfile)
            .FirstOrDefaultAsync(p=>p.Id == id);
        if (profile == null)
        {
            throw new IFEntityNotFoundException("Profile",id);
        }
        return profile;
    }

    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Profile> data, int totalItems)> GetAnyAsync(Expression<Func<Profile, bool>>? predicate = null, int? limit = null, int offset = 0, string? queryOrderBy = null)
    {
        IQueryable<Profile> query =  _dbContext.Profiles
            .Include(p => p.User)
            .Include(p => p.Organisation)
            .Include(p => p.InviterProfile)
            .Include(p => p.SupervisorProfile);
        
        if (predicate != null)
            query = query.Where(predicate);

        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }
        
        var profiles = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (profiles, totalItems);
    }
}