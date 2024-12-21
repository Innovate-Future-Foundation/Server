using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllAsync();
}