using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Enums;
using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class  GetOrganisationsQuery : IRequest<(List<Organisation> data, int totalItems)>
{
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
