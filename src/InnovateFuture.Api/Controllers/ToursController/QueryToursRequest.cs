using InnovateFuture.Application.Common.Models;

namespace InnovateFuture.Api.Controllers.ToursController;

public class QueryToursRequest : IPaginatedRequest<QueryToursFilters>
{
    public QueryToursFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryToursFilters
{
    public Guid? OrgId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
}