using System.Linq.Expressions;
using InnovateFuture.Application.Common.Models;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;

public class GetProfilesHandler : IRequestHandler<GetProfilesQuery, (List<Profile> data, int totalItems)>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfilesHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<(List<Profile> data, int totalItems)> Handle(GetProfilesQuery query, CancellationToken cancellationToken)
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

            if (query.Sortings != null && query.Sortings.Any())
            {
                foreach (var sorting in query.Sortings)
                {
                    string orderBy = sorting.OrderBy;
                    string direction = sorting.IsAscending ? "asc" : "desc";
                    queryOrderBy += string.IsNullOrEmpty(queryOrderBy) ? $"{orderBy} {direction}" : $", {orderBy} {direction}";
                }
            }
        }
        
        var (data, totalItems) = await _profileRepository.GetPagedAsync(queryPredicate, query.Limit, query.Offset ?? 0, queryOrderBy);
        
        return (data, totalItems);
    }
}
