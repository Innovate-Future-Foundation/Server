using System.Linq.Expressions;
using InnovateFuture.Application.Common.Models;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;

public class GetProfilesHandler : IRequestHandler<GetProfilesQuery, PaginatedResult<Profile>>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfilesHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<PaginatedResult<Profile>> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Profile, bool>>? queryPredicate = null;
        string? queryOrderBy = null;
        
        var queriesEmpty = query.GetType().GetProperties().All(p => p.GetValue(query) == null);
        
        if (!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                queryPredicate = p =>
                    (string.IsNullOrEmpty(filters.Email) || p.Email.Contains(filters.Email)) &&
                    (string.IsNullOrEmpty(filters.Name) || p.Name.Contains(filters.Name)) &&
                    (!filters.OrgId.HasValue || p.OrgId == filters.OrgId) &&
                    (!filters.RoleId.HasValue || p.RoleId == filters.RoleId) &&
                    (!filters.IsActive.HasValue || p.IsActive == filters.IsActive);
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                string orderBy = query.OrderBy;
                string direction = query.IsAscending != null ? (query.IsAscending.Value ? "asc" : "desc") : "";
                queryOrderBy = $"{orderBy} {direction}";
            }
        }
        
        var (data, totalItems) = await _profileRepository.GetPagedAsync(queryPredicate, query.Limit, query.Offset ?? 0, queryOrderBy);
        
        return new PaginatedResult<Profile>
        {
            Data = data.ToArray(),
            Meta = new Meta
            {
                Limit = query.Limit,
                TotalItems = totalItems,
            }
        };
    }
}
