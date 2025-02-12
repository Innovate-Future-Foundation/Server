using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;


namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class GetOrganisationsHandler : IRequestHandler<GetOrganisationsQuery, (List<Organisation> data, int totalItems)>
{
    private readonly IOrgRepository _orgRepository;

    public GetOrganisationsHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<(List<Organisation> data, int totalItems)> Handle(GetOrganisationsQuery query, CancellationToken cancellationToken)
    {
        
        var predicate = PredicateBuilder.New<Organisation>(true);
        
        string? queryOrderBy=null;
            
        var queriesEmpty = query.GetType().GetProperties().All(p=>p.GetValue(query)==null);
        
        if(!queriesEmpty)
        {
            if (query.Filters != null)
            {
                var filters = query.Filters;
                // Build predicate based on query conditions
                predicate = predicate.And(o =>
                    (!filters.OrgStatusEnum.HasValue || o.OrgStatus == filters.OrgStatusEnum) &&
                    (!filters.SubscriptionEnum.HasValue || o.Subscription == filters.SubscriptionEnum));
            }

            if (!string.IsNullOrEmpty(query.SearchKey))
            {
                var searchKey = query.SearchKey;
                predicate = predicate.And(o => 
                    EF.Functions.ILike(o.OrgName, $"%{searchKey}%") || 
                    (!string.IsNullOrEmpty(o.Email) && 
                     EF.Functions.ILike(o.Email, $"%{searchKey}%")));
            }

            if (query.Sortings!=null && query.Sortings!.Length>0)
            {
                foreach (var sorting in query.Sortings!)
                {
                    string orderBy = sorting.OrderBy;
                    string direction = sorting.IsAscending ? "asc" : "desc";
                    queryOrderBy += string.IsNullOrEmpty(queryOrderBy)?$"{orderBy} {direction}":$", {orderBy} {direction}";
                }
            }
        }
        
        var (data,totalItems) = await _orgRepository.GetAnyAsync(predicate,query.Limit,query.Offset??0,queryOrderBy);

        return (data, totalItems);
    }
}