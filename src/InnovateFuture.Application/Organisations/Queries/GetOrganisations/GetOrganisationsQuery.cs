using InnovateFuture.Application.Common.Models;
using InnovateFuture.Domain.Enums;
using MediatR;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class  GetOrganisationsQuery : IRequest<PaginatedResult<Organisation>>
{
    public QueryOrganisationsFilters? Filters { get; set; }
    public string? OrderBy { get; set; }
    public bool? IsAscending { get; set; }
    public int? Offset { get; set; }
    public int Limit { get; set; }
}

public class QueryOrganisationsFilters
{
    public string? OrgName { get; set; }
    public StatusEnum? Status { get; set; } 
    public string? Email { get; set; }
}
