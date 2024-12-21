using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        await _dbContext.AddAsync(profile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        _dbContext.Profiles.Update(profile);
        await _dbContext.SaveChangesAsync();
    }
}