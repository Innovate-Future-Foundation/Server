using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Enums;
    
namespace InnovateFuture.Api.Controllers.OrganisationsController;


public class QueryOrganisationsRequest :IPaginatedRequest<QueryOrganisationsFilters> {
    public QueryOrganisationsFilters? Filters { get; set; }
    public Sorting[]? Sortings { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}
public class QueryOrganisationsFilters
{
    public string? OrgNameOrEmail { get; set; }
    public StatusEnum? Status { get; set; } 
    public SubscriptionEnum? Subscription { get; set; }
}
