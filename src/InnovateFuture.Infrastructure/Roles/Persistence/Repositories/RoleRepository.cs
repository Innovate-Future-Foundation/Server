using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Roles.Persistence.Repositories;

public class RoleRepository:IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RoleRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Role> GetByIdAsync(Guid id)
    {
        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.RoleId == id);

        if (role == null)
        {
            throw new IFEntityNotFoundException("Role", id);
        }
        return role;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await _dbContext.Roles
            .ToListAsync();

        if (!roles.Any())
        {
            throw new IFDatabaseException("There are no roles set in the database.");
        }
        return roles;
    }
}