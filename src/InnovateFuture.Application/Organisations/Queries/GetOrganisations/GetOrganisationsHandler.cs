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

    public async Task<List<Organisation>> Handle(GetOrganisationsQuery request, CancellationToken cancellationToken)
    {
        var organisations = new List<Organisation>();
        await _orgRepository.GetAllAsync(organisations);
        return organisations;
    }
}