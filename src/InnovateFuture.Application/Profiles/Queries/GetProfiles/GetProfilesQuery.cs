using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Entities;
using MediatR;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;

public class GetProfilesQuery : IRequest<(List<Profile> data, int totalItems)>
{
    public QueryProfileFilters? Filters { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryProfileFilters
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public Guid? OrgId { get; set; }
    public Guid? RoleId { get; set; }
    public bool? IsActive { get; set; }
}
