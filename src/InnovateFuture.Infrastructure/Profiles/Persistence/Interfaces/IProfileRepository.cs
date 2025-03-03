using InnovateFuture.Domain.Entities;
using System.Linq.Expressions;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

public interface IProfileRepository
{
    Task AddAsync(Profile profile, CancellationToken cancellationToken = default);
    Task<User> GetUserByProfileId(Guid profileId, CancellationToken cancellationToken = default);
    Task CheckProfileExistByUserIdOrgIdRoleAsync(Guid userId, Guid orgId, RoleEnum role,
        CancellationToken cancellationToken = default);
    Task<Profile> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Profile> GetProfileByIdWithOrg(Guid profileId, CancellationToken cancellationToken = default);
    Task UpdateAsync();
    Task<(List<Profile> data, int totalItems)> GetAnyAsync(
        Expression<Func<Profile, bool>>? predicate = null, int? limit = null, int offset = 0,
        string? queryOrderBy = null);
}