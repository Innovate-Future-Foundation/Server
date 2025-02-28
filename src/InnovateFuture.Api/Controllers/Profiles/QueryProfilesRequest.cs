using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.Profiles;

public class QueryProfilesRequest : IPaginatedRequest<QueryProfileFilters>
{
    public QueryProfileFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
    public bool? IncludeDetails { get; set; }
}

public class QueryProfileFilters
{
    public string? RoleCodes { get; set; }
    public Guid? OrgId { get; set; }
    public bool? IsActive { get; set; }
    public string? Supervisors { get; set; }
    public bool? IsConfirmed { get; set; }
}
