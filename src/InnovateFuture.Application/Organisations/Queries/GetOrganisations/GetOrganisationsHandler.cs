using System.Linq.Expressions;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisations;

public class GetOrganisationsHandler : IRequestHandler<GetOrganisationsQuery, List<Organisation>>
{
    private readonly IOrgRepository _orgRepository;

    public GetOrganisationsHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<List<Organisation>> Handle(GetOrganisationsQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<Organisation, bool>> queryPredicate;
        var queriesEmpty = query.GetType().GetProperties().All(p=>p.GetValue(query)==null);
        if (queriesEmpty)
        {
            queryPredicate = o => true; // Fetch all users
        }
        else
        {
            // Build predicate based on query conditions
            queryPredicate = o =>
                (string.IsNullOrEmpty(query.Email) || o.Email == query.Email) &&
                (string.IsNullOrEmpty(query.OrgName) || o.OrgName == query.OrgName) &&
                (!query.Status.HasValue || o.Status == query.Status);
        }

        var organisations = await _orgRepository.GetAnyAsync(queryPredicate);
        
        return organisations.ToList();
    }
}