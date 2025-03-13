using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.DynamicLinq;
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

    public async Task<User> GetUserByProfileId(Guid profileId, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);

        if (profile == null)
        {
            throw new IFEntityNotFoundException("Profile", profileId);
        }
        return profile.User;
    }

    public async Task<Guid> GetOrgIdByProfileIdAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Where(p => p.Id == profileId)
            .Select(p => new { p.OrgId })
            .FirstOrDefaultAsync(cancellationToken);

        if (profile == null || profile.OrgId == null)
        {
            throw new IFEntityNotFoundException("OrgId in Profile", profileId);
        }
        return profile.OrgId.Value;
    }

    public async Task<Profile> GetProfileByIdWithOrg(Guid profileId, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
            .Include(p => p.Organisation)
            .FirstOrDefaultAsync(p => p.Id == profileId, cancellationToken);
        if (profile == null)
        {
            throw new IFEntityNotFoundException("Profile",profileId);
        }
        return profile;
    }

    public async Task<bool> CanInviteRole(Guid id, RoleEnum inviteeRoleEnum, CancellationToken cancellationToken = default)
    {
       var inviter = await _dbContext.Profiles
           .Where(p => p.Id == id)
           .Select(p => new { p.Role })
           .FirstOrDefaultAsync(cancellationToken);
       
       if (inviter == null)
       {
           throw new IFEntityNotFoundException("ProfileRole", id);
       }

       return inviter.Role switch
       {
           RoleEnum.OrgAdmin => inviteeRoleEnum is RoleEnum.OrgManager or RoleEnum.OrgTeacher or RoleEnum.Parent or RoleEnum.Student,
           RoleEnum.OrgManager => inviteeRoleEnum is RoleEnum.OrgTeacher or RoleEnum.Parent or RoleEnum.Student,
           RoleEnum.OrgTeacher => inviteeRoleEnum is RoleEnum.Parent or RoleEnum.Student,
           _ => false 
       };
    }

    public async Task<Profile> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.Profiles
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