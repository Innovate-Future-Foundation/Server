using System.Threading.Tasks;
using System.Threading;
using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Exceptions;

namespace InnovateFuture.Application.Organisations.Queries.GetOrganisation
{
    public class GetOrganisationHandler : IRequestHandler<GetOrganisationQuery, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationQuery request, CancellationToken cancellationToken)
        {
            var organisation = await _organisationRepository.GetByIdAsync(request.OrganisationId);
            
            if (organisation == null)
            {
                throw new IFEntityNotFoundException(nameof(Organisation), request.OrganisationId);
            }

            return organisation;
        }
    }
}
