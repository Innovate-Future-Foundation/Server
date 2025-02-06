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
                
                var roleIds = filters.RoleIds?.Split(",")??[];
                
                var validGuids = roleIds
                    .Where(s => Guid.TryParse(s, out _))
                    .Select(Guid.Parse)
                    .ToList();
                
                queryPredicate = p =>
                    (string.IsNullOrEmpty(filters.NameOrEmailOrPhone) || 
                     (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(filters.NameOrEmailOrPhone)) ||
                    (!string.IsNullOrEmpty(p.Email) && p.Email.Contains(filters.NameOrEmailOrPhone)) ||
                    (!string.IsNullOrEmpty(p.Phone) && p.Phone.Contains(filters.NameOrEmailOrPhone))) &&
                    (filters.OrgId==null || p.OrgId==filters.OrgId) &&
                    (validGuids.Contains(p.RoleId)) &&
                    (!filters.IsConfirmed.HasValue || p.IsConfirmed == filters.IsConfirmed) &&
                    (!filters.IsActive.HasValue || p.IsActive == filters.IsActive);
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
        
        var (data, totalItems) = await _profileRepository.GetAnyAsync(queryPredicate, query.Limit, query.Offset ?? 0, queryOrderBy);
        
        return (data, totalItems);
    }
}
