using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Enums;
    
namespace InnovateFuture.Api.Controllers.OrganisationsController;


public class QueryOrganisationsRequest :IPaginatedRequest<QueryOrganisationsFilters> {
    public QueryOrganisationsFilters? Filters { get; set; }
    public string? SearchKey { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}
public class QueryOrganisationsFilters
{
    public string? OrgStatusCode { get; set; } 
    public string? SubscriptionCode { get; set; }
}
