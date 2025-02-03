using InnovateFuture.Domain.Entities;
using System.Linq.Expressions;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

public interface IProfileRepository
{
    Task<Profile> GetByIdAsync(Guid id);
    Task AddAsync(Profile profile);
    Task UpdateAsync();
    
    Task<(List<Profile> Data, int TotalItems)> GetPagedAsync(
        Expression<Func<Profile, bool>>? predicate = null,
        int? limit = null,
        int offset = 0,
        string? orderBy = null);
        
}