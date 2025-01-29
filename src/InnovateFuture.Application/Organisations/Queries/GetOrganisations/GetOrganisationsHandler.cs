using System.Linq.Expressions;
using InnovateFuture.Application.Common.Models;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class GetOrganisationsHandler : IRequestHandler<GetOrganisationsQuery, PaginatedResult<Organisation>>
{
    private readonly IOrgRepository _orgRepository;

    public GetOrganisationsHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<PaginatedResult<Organisation>> Handle(GetOrganisationsQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Organisation, bool>>? queryPredicate=null;
        string? queryOrderBy=null;
            
        var queriesEmpty = query.GetType().GetProperties().All(p=>p.GetValue(query)==null);
        
        if(!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                // Build predicate based on query conditions
                queryPredicate = o =>
                    (string.IsNullOrEmpty(filters.Email) || o.Email.Contains(filters.Email)) &&
                    (string.IsNullOrEmpty(filters.OrgName) || o.OrgName.Contains(filters.OrgName)) &&
                    (!filters.Status.HasValue || o.Status == filters.Status);
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                string orderBy = query.OrderBy;
                string direction = query.IsAscending != null ? (query.IsAscending.Value ? "asc" : "desc") : "";
                queryOrderBy = $"{orderBy} {direction}";
            }
        }
        
        var (data,totalItems) = await _orgRepository.GetAnyAsync(queryPredicate,query.Limit,query.Offset??0,queryOrderBy);
        
        return new PaginatedResult<Organisation>
        {
            Data= data.ToArray(),
            Meta= new Meta
            {
                PageSize=query.Limit,
                TotalItems=totalItems,
            }
        };
    }
}