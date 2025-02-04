using InnovateFuture.Domain.Entities;
using System.Linq.Expressions;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

public interface IProfileRepository
{
    Task<Profile> GetByIdAsync(Guid id);
    Task AddAsync(Profile profile);
    Task UpdateAsync();
    Task<(List<Profile> data, int totalItems)> GetAnyAsync(
        Expression<Func<Profile, bool>>? predicate = null, int? limit = null, int offset = 0,
        string? queryOrderBy = null);
}