using InnovateFuture.Domain.Enums;
    
namespace InnovateFuture.Api.Controllers.OrganisationsController;


public class QueryOrganisationsRequest{
    public QueryOrganisationsFilters? Filters { get; set; }
    public string? OrderBy { get; set; }
    public bool? IsAscending { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}
public class QueryOrganisationsFilters
{
    public string? OrgNameOrEmail { get; set; }
    public StatusEnum? Status { get; set; } 
    public SubscriptionEnum? Subscription { get; set; }
}