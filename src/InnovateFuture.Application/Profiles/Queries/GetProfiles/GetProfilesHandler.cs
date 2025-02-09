using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using LinqKit;

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
        var predicate = PredicateBuilder.New<Profile>(true);
        string? queryOrderBy = null;
        
        var queriesEmpty = query.GetType().GetProperties().All(p => p.GetValue(query) == null);
        
        if (!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                
                
                predicate = predicate.And(p =>
                    (filters.OrgId==null || p.OrgId==filters.OrgId) &&
                    (filters.RoleEnums.Length==0 || filters.RoleEnums.Contains(p.Role)) &&
                    (filters.Supervisor == null|| p.Supervisor == filters.Supervisor) &&
                    (!filters.IsConfirmed.HasValue || p.IsConfirmed == filters.IsConfirmed) &&
                    (!filters.IsActive.HasValue || p.IsActive == filters.IsActive));
            }

            if (!string.IsNullOrEmpty(query.SearchKey))
            {
                var searchKeys = query.SearchKey;
                predicate = predicate.And((p=> 
                    (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchKeys)) || 
                    (!string.IsNullOrEmpty(p.Email) && p.Email.Contains(searchKeys)) || 
                    (!string.IsNullOrEmpty(p.Phone) && p.Phone.Contains(searchKeys))));
            }
            if (query.Sortings != null && query.Sortings!.Length>0)
            {
                foreach (var sorting in query.Sortings)
                {
                    string orderBy = sorting.OrderBy;
                    string direction = sorting.IsAscending ? "asc" : "desc";
                    queryOrderBy += string.IsNullOrEmpty(queryOrderBy) ? $"{orderBy} {direction}" : $", {orderBy} {direction}";
                }
            }
        }
        
        var (data, totalItems) = await _profileRepository.GetAnyAsync(predicate, query.Limit, query.Offset ?? 0, queryOrderBy);
        
        return (data, totalItems);
    }
}
