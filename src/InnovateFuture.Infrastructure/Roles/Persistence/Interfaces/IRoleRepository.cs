using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Roles.Persistence.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(Guid id);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Guid id);
    }
}
