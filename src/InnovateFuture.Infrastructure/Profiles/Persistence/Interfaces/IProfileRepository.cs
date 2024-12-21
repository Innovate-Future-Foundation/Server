using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

public interface IProfileRepository
{
    Task<Profile> GetByIdAsync(Guid id);
    Task AddAsync(Profile profile);
    Task UpdateAsync(Profile profile);
}