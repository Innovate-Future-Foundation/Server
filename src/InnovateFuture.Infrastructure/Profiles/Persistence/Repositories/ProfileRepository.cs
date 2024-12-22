using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Repositories
{
    public class ProfileRepository : IProfileRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //CRUD
        //public async Task<Profile> GetByIdAsync(long profileId)
        //{
        //    return await _dbContext.Profiles.FindAsync(profileId);
        //}

        //public async Task AddAsync(Profile profile)
        //{
        //    await _dbContext.Profiles.AddAsync(profile);
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(Profile profile)
        //{
        //    _dbContext.Profiles.Update(profile);
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task DeleteAsync(long profileId)
        //{
        //    var profile = await _dbContext.Profiles.FindAsync(profileId);
        //    if (profile != null)
        //    {
        //        _dbContext.Profiles.Remove(profile);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //}
    }
}
