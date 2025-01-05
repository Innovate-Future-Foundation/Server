using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Users.Persistence.Repositories;

public class UserRepository:IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .Include(u => u.Profiles)
            .ThenInclude(p => p.Organisation)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.Role)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.InvitedByProfile)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.SupervisedByProfile)
            .FirstOrDefaultAsync(u => u.UserId == id);
        
        if (user == null)
        {
            throw new IFEntityNotFoundException("user",id);
        }
        return user;
    }
    
    public async Task<IEnumerable<User>> GetAnyAsync(Expression<Func<User, bool>> predicate)
    {
        var users = await _dbContext.Users
            .Include(u => u.Profiles)
            .ThenInclude(p => p.Organisation)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.Role)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.InvitedByProfile)
            .Include(u => u.Profiles)
            .ThenInclude(p => p.SupervisedByProfile)
            .Where(predicate)
            .ToListAsync();
        
        return users;
    }
    
    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}