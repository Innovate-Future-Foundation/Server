using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using MediatR;

namespace InnovateFuture.Application.Profiles.Queries.GetProfiles;

public class GetProfilesQuery : IRequest<(List<Profile> data, int totalItems)>
{
    public QueryProfileFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryProfileFilters
{
    public RoleEnum[] RoleEnums { get; set; } = [];
    public Guid? OrgId { get; set; }
    public Guid? Supervisor { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsConfirmed { get; set; }
}
