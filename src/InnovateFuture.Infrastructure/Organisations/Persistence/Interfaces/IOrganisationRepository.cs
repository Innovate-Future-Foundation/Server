using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
public interface IOrganisationRepository
{
	Task<Organisation> GetByIdAsync(Guid id);
	Task AddAsync(Organisation organisation);
}

