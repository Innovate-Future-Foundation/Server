using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisation;

public class GetOrganisationHandler : IRequestHandler<GetOrganisationQuery, Organisation>
{
    private readonly IOrgRepository _orgRepository;

    public GetOrganisationHandler(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    public async Task<Organisation> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
    {
        return await _orgRepository.GetByIdAsync(request.OrgId);
    }
}