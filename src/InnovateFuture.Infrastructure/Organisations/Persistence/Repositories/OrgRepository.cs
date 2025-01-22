using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Repositories;

public class OrgRepository:IOrgRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrgRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Organisation> GetByIdAsync(Guid id)
    {
        var org = await _dbContext.Organisations
            .Include(o => o.Profiles)
            .FirstOrDefaultAsync(o=>o.OrgId == id);
        if (org == null)
        {
            throw new IFEntityNotFoundException("Organisation", id);
        }
        return org;
    }

    public async Task AddAsync(Organisation organisation)
    {
        await _dbContext.Organisations.AddAsync(organisation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Organisation organisation)
    {
        _dbContext.Organisations.Update(organisation);
        await _dbContext.SaveChangesAsync();
    }
}