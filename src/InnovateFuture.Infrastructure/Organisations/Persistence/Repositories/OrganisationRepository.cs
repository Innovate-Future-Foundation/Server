using Microsoft.EntityFrameworkCore;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Exceptions;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrganisationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Organisation> GetByIdAsync(Guid id)
        {
            var organisation = await _dbContext.Organisations
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (organisation == null)
            {
                throw new IFEntityNotFoundException("Organisation", id);
            }
            
            return organisation;
        }

        public async Task AddAsync(Organisation organisation)
        {
            await _dbContext.Organisations.AddAsync(organisation);
            await _dbContext.SaveChangesAsync();
        }
    }
}