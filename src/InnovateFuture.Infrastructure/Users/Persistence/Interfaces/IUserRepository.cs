using System.Linq.Expressions;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task AddAsync(User user); 
    Task UpdateAsync();
    Task<IEnumerable<User>>GetAnyAsync(Expression<Func<User, bool>> predicate);
}