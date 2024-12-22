using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>>GetAnyAsync(Expression<Func<Role, bool>> predicate);
}