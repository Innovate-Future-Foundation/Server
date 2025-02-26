using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Repositories;

public class OrgRepository:IOrgRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrgRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task AddAsync(Organisation organisation, CancellationToken cancellationToken = default)
    {
            await _dbContext.Organisations.AddAsync(organisation, cancellationToken);
    }

    public async Task CheckIsExistByNameOrEmailAsync(string orgName, string? orgEmail,
        CancellationToken cancellationToken = default)
    {
        var existingOrg = await _dbContext.Organisations
            .Where(o => o.OrgName == orgName || (orgEmail != null && o.Email == orgEmail))
            .Select(o => new { o.OrgName, o.Email })
            .FirstOrDefaultAsync(cancellationToken);
        
        if (existingOrg != null)
        {
            if (!string.IsNullOrWhiteSpace(orgEmail) && existingOrg.Email == orgEmail)
            {
                throw new IFConcurrencyException($"The organisation email {orgEmail} already exists.");
            }
            if (existingOrg.OrgName == orgName)
            {
                throw new IFConcurrencyException($"The organisation name {orgName} already exists.");
            }
        }
    }
    
    
    public async Task<Organisation> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var org = await _dbContext.Organisations
            .Include(o => o.Profiles)
            .FirstOrDefaultAsync(o=>o.Id == id);
        if (org == null)
        {
            throw new IFEntityNotFoundException("Organisation", id);
        }
        return org;
    }
    
    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Organisation> data, int totalItems)> GetAnyAsync(Expression<Func<Organisation, bool>>? predicate=null, int? limit = null, int offset=0,string? queryOrderBy=null)
    {
        IQueryable<Organisation> query = _dbContext.Organisations;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        
        int totalItems = await query.CountAsync();
        
        if (!string.IsNullOrEmpty(queryOrderBy))
        {
            query = query.OrderBy(queryOrderBy);
        }

        var organisations = await query.Skip(offset)
            .Take(limit ?? totalItems).ToListAsync();
        
        return (organisations, totalItems);
    }
}