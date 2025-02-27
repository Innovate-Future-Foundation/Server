using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.ActivitiesController;

public class QueryActivityRequest
{
    public QueryActivityFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryActivityFilters
{
    public Guid? OrgId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}